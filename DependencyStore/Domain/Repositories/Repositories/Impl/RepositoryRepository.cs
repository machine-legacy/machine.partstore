using System;
using System.Collections.Generic;
using System.IO;

using Machine.Core.Services;
using Machine.Core.Utility;

using DependencyStore.Domain.Core;
using DependencyStore.Domain.Configuration.Repositories;

namespace DependencyStore.Domain.Repositories.Repositories.Impl
{
  public class RepositoryRepository : IRepositoryRepository
  {
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
        return new Repository();
      }
      using (StreamReader stream = new StreamReader(_fileSystem.OpenFile(path.AsString)))
      {
        return Prepare(XmlSerializationHelper.DeserializeString<Repository>(stream.ReadToEnd()), path);
      }
    }

    public void SaveRepository(Repository repository)
    {
      Purl path = _currentConfiguration.DefaultConfiguration.RepositoryDirectory.Join("Manifest.xml");
      using (StreamWriter stream = new StreamWriter(_fileSystem.CreateFile(path.AsString)))
      {
        stream.Write(XmlSerializationHelper.Serialize(repository));
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
          version.ArchivePath = repository.RootPath.Join(version.ArchiveFileName);
        }
      }
      return repository;
    }
  }
}
