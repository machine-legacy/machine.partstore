using System;
using System.Collections.Generic;

using DependencyStore.Domain;

namespace DependencyStore.Services.DataAccess
{
  public interface ILocationRepository
  {
    IList<SourceLocation> FindAllSources(FileAndDirectoryRules rules);
    IList<SinkLocation> FindAllSinks(FileAndDirectoryRules rules);
  }
}
