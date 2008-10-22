using System;
using System.Collections.Generic;
using System.IO;

using Machine.Core.Services;
using Machine.Core.Utility;

using Machine.Partstore.Domain.FileSystem;
using Machine.Partstore.Domain.FileSystem.Repositories;

namespace Machine.Partstore.Domain.Configuration.Repositories.Impl
{
  public class ConfigurationRepository : IConfigurationRepository
  {
    private static readonly XmlSerializer<PartstoreConfiguration> _serializer = new XmlSerializer<PartstoreConfiguration>();
    private readonly IFileSystem _fileSystem;
    private readonly IFileAndDirectoryRulesRepository _fileAndDirectoryRulesRepository;
    private readonly ConfigurationPaths _paths;

    public ConfigurationRepository(IFileSystem fileSystem, IFileAndDirectoryRulesRepository fileAndDirectoryRulesRepository, ConfigurationPaths paths)
    {
      _fileSystem = fileSystem;
      _fileAndDirectoryRulesRepository = fileAndDirectoryRulesRepository;
      _paths = paths;
    }

    #region IConfigurationRepository Members
    public PartstoreConfiguration FindAndRequireProjectConfiguration()
    {
      PartstoreConfiguration configuration = FindProjectConfiguration();
      if (configuration == null)
      {
        throw new InvalidOperationException("Missing configuration!");
      }
      return configuration;
    }

    public PartstoreConfiguration FindProjectConfiguration()
    {
      string path = _paths.FindConfigurationForCurrentProjectPath();
      PartstoreConfiguration configuration = ReadConfiguration(path);
      if (configuration == null)
      {
        return null;
      }
      return configuration;
    }

    public void SaveProjectConfiguration(PartstoreConfiguration configuration)
    {
      string path = _paths.InferPathToConfigurationForCurrentProject();
      if (String.IsNullOrEmpty(path))
      {
        throw new InvalidOperationException("Unable to infer project root directory.");
      }
      configuration.ConfigurationPath = new Purl(path);
      using (StreamWriter writer = _fileSystem.CreateText(path))
      {
        writer.Write(_serializer.Serialize(configuration));
      }
    }
    #endregion

    private PartstoreConfiguration ReadConfiguration(string path)
    {
      if (path == null || !_fileSystem.IsFile(path))
      {
        return null;
      }
      try
      {
        using (StreamReader reader = _fileSystem.OpenText(path))
        {
          PartstoreConfiguration configuration = _serializer.DeserializeString(reader.ReadToEnd());
          configuration.FileAndDirectoryRules = _fileAndDirectoryRulesRepository.FindDefault();
          configuration.ConfigurationPath = new Purl(path);
          configuration.EnsureValid();
          return configuration;
        }
      }
      catch (Exception e)
      {
        throw new InvalidConfigurationException("Error reading configuration", e);
      }
    }
  }
}
