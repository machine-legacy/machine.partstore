using System;
using System.Collections.Generic;
using System.IO;

using Machine.Core.Services;
using Machine.Core.Utility;

using DependencyStore.Domain.FileSystem;
using DependencyStore.Domain.FileSystem.Repositories;

namespace DependencyStore.Domain.Configuration.Repositories.Impl
{
  public class ConfigurationRepository : IConfigurationRepository
  {
    private static readonly XmlSerializer<DependencyStoreConfiguration> _serializer = new XmlSerializer<DependencyStoreConfiguration>();
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
    public DependencyStoreConfiguration FindProjectConfiguration()
    {
      string path = _paths.FindConfigurationForCurrentProjectPath();
      DependencyStoreConfiguration configuration = FindConfiguration(path);
      if (configuration.ProjectConfigurations.Count == 0)
      {
        Purl rootPath = new Purl(path).Parent;
        ProjectStructure projectStructure = new ProjectStructure(_fileSystem, rootPath);
        configuration.ProjectConfigurations.Add(projectStructure.InferProjectConfiguration());
      }
      return configuration;
    }
    #endregion

    private DependencyStoreConfiguration FindConfiguration(string configurationFile)
    {
      try
      {
        if (configurationFile == null)
        {
          throw new FileNotFoundException();
        }
        using (StreamReader reader = _fileSystem.OpenText(configurationFile))
        {
          DependencyStoreConfiguration configuration = _serializer.DeserializeString(reader.ReadToEnd());
          configuration.FileAndDirectoryRules = _fileAndDirectoryRulesRepository.FindDefault();
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
