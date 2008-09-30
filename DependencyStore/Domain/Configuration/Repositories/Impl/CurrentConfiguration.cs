using System;
using System.Collections.Generic;

namespace DependencyStore.Domain.Configuration.Repositories.Impl
{
  public class CurrentConfiguration : ICurrentConfiguration
  {
    private readonly IConfigurationRepository _configurationRepository;
    private DependencyStoreConfiguration _defaultConfiguration;

    public CurrentConfiguration(IConfigurationRepository configurationRepository)
    {
      _configurationRepository = configurationRepository;
    }

    #region ICurrentConfiguration Members
    public DependencyStoreConfiguration DefaultConfiguration
    {
      get
      {
        if (_defaultConfiguration == null)
        {
          _defaultConfiguration = _configurationRepository.FindAndRequireProjectConfiguration();
        }
        return _defaultConfiguration;
      }
    }
    #endregion
  }
}
