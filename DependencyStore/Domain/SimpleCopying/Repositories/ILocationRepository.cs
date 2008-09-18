using System;
using System.Collections.Generic;

namespace DependencyStore.Domain.SimpleCopying.Repositories
{
  public interface ILocationRepository
  {
    IList<SourceLocation> FindAllSources();
    IList<SinkLocation> FindAllSinks();
  }
}
