using System;
using System.Collections.Generic;
using System.IO;

using Machine.Core.Services;
using Machine.Core.Utility;

using DependencyStore.Domain.Core;
using DependencyStore.Domain.Configuration.Repositories;

namespace DependencyStore.Domain.Distribution.Repositories.Impl
{
  public class RepositoryRepository : IRepositoryRepository
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(ProjectRepository));
    private static readonly XmlSerializer<Repository> _serializer = new XmlSerializer<Repository>();
    private readonly ICurrentConfiguration _currentConfiguration;
    private readonly IFileSystem _fileSystem;

    public RepositoryRepository(ICurrentConfiguration currentConfiguration, IFileSystem fileSystem)
    {
      _currentConfiguration = currentConfiguration;
      _fileSystem = fileSystem;
    }

    #region IRepositoryRepository Members
    public Repository FindDefaultRepository()
    {
      Purl path = _currentConfiguration.DefaultConfiguration.RepositoryDirectory.Join("Manifest.xml");
      if (!_fileSystem.IsFile(path.AsString))
      {
        Console.WriteLine("Creating new repository: " + path.AsString);
        _log.Info("Creating new repository: " + path.AsString);
        return Hydrate(new Repository(), path);
      }
      _log.Info("Opening: " + path.AsString);
      using (StreamReader stream = new StreamReader(_fileSystem.OpenFile(path.AsString)))
      {
        return Hydrate(_serializer.DeserializeString(stream.ReadToEnd()), path);
      }
    }

    public void SaveRepository(Repository repository)
    {
      IEnumerable<ProjectVersionAdded> changes = FindAllChanges(repository);
      CommitChanges(changes);
      SaveRepositoryManifest(repository);
      RunChangeHooks(repository, changes);
    }

    public void RefreshRepository(Repository repository)
    {
      Hooks.Create(repository).RunRefresh();
    }
    #endregion

    private static Repository Hydrate(Repository repository, Purl rootPath)
    {
      repository.RootPath = rootPath.Parent;
      foreach (ArchivedProject project in repository.Projects)
      {
        foreach (ArchivedProjectVersion version in project.Versions)
        {
          version.PathInRepository = repository.RootPath.Join(version.RepositoryAlias);
        }
      }
      return repository;
    }

    private static IEnumerable<ProjectVersionAdded> FindAllChanges(Repository repository)
    {
      List<ProjectVersionAdded> changes = new List<ProjectVersionAdded>();
      foreach (ArchivedProject project in repository.Projects)
      {
        foreach (ArchivedProjectVersion version in project.Versions)
        {
          if (!Repository.AccessStrategy.IsVersionPresentInRepository(version))
          {
            _log.Info("New version of " + project + " is " + version);
            changes.Add(new ProjectVersionAdded(project, version));
          }
        }
      }
      return changes;
    }

    private static void CommitChanges(IEnumerable<ProjectVersionAdded> changes)
    {
      foreach (ProjectVersionAdded change in changes)
      {
        Repository.AccessStrategy.CommitVersionToRepository(new NewProjectVersion(change));
      }
    }

    private void SaveRepositoryManifest(Repository repository)
    {
      Purl path = _currentConfiguration.DefaultConfiguration.RepositoryDirectory.Join("Manifest.xml");
      _log.Info("Saving: " + path.AsString);
      using (StreamWriter stream = new StreamWriter(_fileSystem.CreateFile(path.AsString)))
      {
        stream.Write(_serializer.Serialize(repository));
      }
    }

    private static void RunChangeHooks(Repository repository, IEnumerable<ProjectVersionAdded> changes)
    {
      foreach (ProjectVersionAdded change in changes)
      {
        Hooks.Create(repository).RunCommit(change.Project, change.Version);
      }
    }
  }
}
