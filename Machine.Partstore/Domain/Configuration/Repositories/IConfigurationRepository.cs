using System;
using System.Collections.Generic;

namespace Machine.Partstore.Domain.Configuration.Repositories
{
  public interface IConfigurationRepository
  {
    PartstoreConfiguration FindAndRequireProjectConfiguration();
    PartstoreConfiguration FindProjectConfiguration();
    void SaveProjectConfiguration(PartstoreConfiguration configuration);
  }
}
