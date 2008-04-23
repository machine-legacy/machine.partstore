using System;
using System.Collections.Generic;

using DependencyStore.Domain;

namespace DependencyStore.Services.DataAccess
{
  public interface ILocationRepository
  {
    IList<Location> FindAll();
    IList<Location> FindAllSources();
    IList<Location> FindAllSinks();
  }
}
