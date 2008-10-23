using System;
using System.Collections.Generic;

using Machine.Partstore.Domain.FileSystem;
using Machine.Partstore.Domain.FileSystem.Repositories;

namespace Machine.Partstore.Domain.Configuration.Repositories.Impl
{
  public class CurrentConfiguration : ICurrentConfiguration
  {
    private readonly IConfigurationRepository _configurationRepository;
    private readonly IFileSystemEntryRepository _fileSystemEntryRepository;
    private PartstoreConfiguration _defaultConfiguration;

    public CurrentConfiguration(IConfigurationRepository configurationRepository, IFileSystemEntryRepository fileSystemEntryRepository)
    {
      _configurationRepository = configurationRepository;
      _fileSystemEntryRepository = fileSystemEntryRepository;
    }

    #region ICurrentConfiguration Members
    public PartstoreConfiguration DefaultConfiguration
    {
      get
      {
        if (_defaultConfiguration == null)
        {
          _defaultConfiguration = _configurationRepository.FindAndRequireProjectConfiguration();
          if (_defaultConfiguration.ProjectConfigurations.Count == 0)
          {
            Purl rootPath = _defaultConfiguration.ConfigurationPath.Parent;
            ProjectStructure projectStructure = new ProjectStructure(rootPath);
            _defaultConfiguration.ProjectConfigurations.Add(projectStructure.InferProjectConfiguration());
          }
        }
        return _defaultConfiguration;
      }
    }
    #endregion
  }
}
