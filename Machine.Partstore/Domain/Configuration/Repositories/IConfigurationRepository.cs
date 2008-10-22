using System;
using System.Collections.Generic;

namespace Machine.Partstore.Domain.Configuration.Repositories
{
  public interface IConfigurationRepository
  {
    DependencyStoreConfiguration FindAndRequireProjectConfiguration();
    DependencyStoreConfiguration FindProjectConfiguration();
    void SaveProjectConfiguration(DependencyStoreConfiguration configuration);
  }
}
