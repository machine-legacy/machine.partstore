using System;
using System.Collections.Generic;

using DependencyStore.Domain;
using DependencyStore.Domain.Configuration;

namespace DependencyStore.Services.DataAccess
{
  public interface ILocationRepository
  {
    IList<SourceLocation> FindAllSources(DependencyStoreConfiguration configuration, FileAndDirectoryRules rules);
    IList<SinkLocation> FindAllSinks(DependencyStoreConfiguration configuration, FileAndDirectoryRules rules);
  }
}
