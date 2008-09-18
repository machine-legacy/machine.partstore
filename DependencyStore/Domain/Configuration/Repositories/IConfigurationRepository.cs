using System;
using System.Collections.Generic;

namespace DependencyStore.Domain.Configuration.Repositories
{
  public interface IConfigurationRepository
  {
    DependencyStoreConfiguration FindProjectConfiguration();
  }
}
