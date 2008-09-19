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
        Console.WriteLine("Creating new repository, {0} is missing.", path.AsString);
        return new Repository();
      }
      using (StreamReader stream = new StreamReader(_fileSystem.OpenFile(path.AsString)))
      {
        return Prepare(_serializer.DeserializeString(stream.ReadToEnd()), path);
      }
    }

    public void SaveRepository(Repository repository)
    {
      Purl path = _currentConfiguration.DefaultConfiguration.RepositoryDirectory.Join("Manifest.xml");
      using (StreamWriter stream = new StreamWriter(_fileSystem.CreateFile(path.AsString)))
      {
        stream.Write(_serializer.Serialize(repository));
      }
    }
    #endregion

    private static Repository Prepare(Repository repository, Purl rootPath)
    {
      repository.RootPath = rootPath.Parent;
      foreach (ArchivedProject project in repository.Projects)
      {
        foreach (ArchivedProjectVersion version in project.Versions)
        {
          version.ArchivePath = repository.RootPath.Join(version.RepositoryAlias);
        }
      }
      return repository;
    }
  }
}
