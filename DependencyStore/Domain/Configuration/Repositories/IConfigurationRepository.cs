using System;
using System.Collections.Generic;

using DependencyStore.Domain.Configuration;

namespace DependencyStore.Domain.Configuration.Repositories
{
  public interface IConfigurationRepository
  {
    DependencyStoreConfiguration FindDefaultConfiguration();
    DependencyStoreConfiguration FindProjectConfiguration();
  }
}
