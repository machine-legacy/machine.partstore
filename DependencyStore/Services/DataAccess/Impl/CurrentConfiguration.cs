using System;
using System.Collections.Generic;

using DependencyStore.Domain.Configuration;

namespace DependencyStore.Services.DataAccess.Impl
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
          _defaultConfiguration = _configurationRepository.FindDefaultConfiguration();
        }
        return _defaultConfiguration;
      }
    }
    #endregion
  }
}
