using System;
using System.Collections.Generic;

namespace DependencyStore.Domain.Configuration.Repositories
{
  public interface ICurrentConfiguration
  {
    DependencyStoreConfiguration DefaultConfiguration
    {
      get;
    }
  }
}
