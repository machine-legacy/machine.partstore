using System;
using System.Collections.Generic;
using System.IO;

using DependencyStore.Domain;
using DependencyStore.Domain.Configuration;

using Machine.Core.Services;
using Machine.Core.Utility;

namespace DependencyStore.Services.DataAccess.Impl
{
  public class RepositoryRepository : IRepositoryRepository
  {
    private readonly IFileSystem _fileSystem;

    public RepositoryRepository(IFileSystem fileSystem)
    {
      _fileSystem = fileSystem;
    }

    #region IRepositoryRepository Members
    public Repository FindDefaultRepository(DependencyStoreConfiguration configuration)
    {
      Purl path = configuration.RepositoryDirectory.Join("Manifest.xml");
      if (!_fileSystem.IsFile(path.AsString))
      {
        return new Repository();
      }
      using (StreamReader stream = new StreamReader(_fileSystem.OpenFile(path.AsString)))
      {
        return XmlSerializationHelper.DeserializeString<Repository>(stream.ReadToEnd());
      }
    }

    public void SaveRepository(Repository repository, DependencyStoreConfiguration configuration)
    {
      Purl path = configuration.RepositoryDirectory.Join("Manifest.xml");
      using (StreamWriter stream = new StreamWriter(_fileSystem.CreateFile(path.AsString)))
      {
        stream.Write(XmlSerializationHelper.Serialize(repository));
      }
    }
    #endregion
  }
}
