using System;
using System.Collections.Generic;

using DependencyStore.Domain.Core;

namespace DependencyStore.Services.DataAccess
{
  public interface ILocationRepository
  {
    IList<SourceLocation> FindAllSources();
    IList<SinkLocation> FindAllSinks();
  }
}
