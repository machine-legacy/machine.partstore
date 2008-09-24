using System;
using System.Collections.Generic;
using System.IO;

using Machine.Core.Services;
using Machine.Core.Utility;

using DependencyStore.Domain.Core;
using DependencyStore.Domain.Configuration.Repositories;
using DependencyStore.Domain.Services;

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
        return Prepare(new Repository(), path);
      }
      _log.Info("Opening: " + path.AsString);
      using (StreamReader stream = new StreamReader(_fileSystem.OpenFile(path.AsString)))
      {
        return Prepare(_serializer.DeserializeString(stream.ReadToEnd()), path);
      }
    }

    public void SaveRepository(Repository repository)
    {
      List<ProjectVersionCommitted> changes = new List<ProjectVersionCommitted>();
      foreach (ArchivedProject project in repository.Projects)
      {
        foreach (ArchivedProjectVersion version in project.Versions)
        {
          if (!Repository.AccessStrategy.IsVersionPresentInRepository(version))
          {
            changes.Add(new ProjectVersionCommitted(project, version));
          }
        }
      }
      foreach (ProjectVersionCommitted change in changes)
      {
        new AddingNewVersionsToRepository().CommitNewVersion(change);
      }
      SaveRepositoryManifest(repository);
      foreach (ProjectVersionCommitted change in changes)
      {
        Hooks.Create(repository).RunCommit(change.Project, change.Version);
      }
    }
    #endregion

    private void SaveRepositoryManifest(Repository repository)
    {
      Purl path = _currentConfiguration.DefaultConfiguration.RepositoryDirectory.Join("Manifest.xml");
      _log.Info("Saving: " + path.AsString);
      using (StreamWriter stream = new StreamWriter(_fileSystem.CreateFile(path.AsString)))
      {
        stream.Write(_serializer.Serialize(repository));
      }
    }

    private static Repository Prepare(Repository repository, Purl rootPath)
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
  }
}
