using System;
using System.Collections.Generic;
using System.IO;

using Machine.Core.Services;
using Machine.Core.Utility;

using DependencyStore.Domain.FileSystem;

namespace DependencyStore.Domain.Core.Repositories.Impl
{
  public class RepositoryRepository : IRepositoryRepository
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(RepositoryRepository));
    private static readonly XmlSerializer<Repository> _serializer = new XmlSerializer<Repository>();
    private readonly IFileSystem _fileSystem;

    public RepositoryRepository(IFileSystem fileSystem)
    {
      _fileSystem = fileSystem;
    }

    #region IRepositoryRepository Members
    public Repository FindRepository(Purl path)
    {
      Purl manifestPath = path.Join("Manifest.xml");
      if (!_fileSystem.IsFile(manifestPath.AsString))
      {
        Console.WriteLine("Creating new repository: " + path.AsString);
        _log.Info("Creating new repository: " + path.AsString);
        return Hydrate(new Repository(), manifestPath);
      }
      _log.Info("Opening: " + path.AsString);
      using (StreamReader stream = new StreamReader(_fileSystem.OpenFile(manifestPath.AsString)))
      {
        return Hydrate(_serializer.DeserializeString(stream.ReadToEnd()), manifestPath);
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

    private static Repository Hydrate(Repository repository, Purl manifestPath)
    {
      repository.RootPath = manifestPath.Parent;
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
            changes.Add(new ProjectVersionAdded(new ArchivedProjectAndVersion(project, version)));
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
      Purl path = repository.RootPath.Join("Manifest.xml");
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
