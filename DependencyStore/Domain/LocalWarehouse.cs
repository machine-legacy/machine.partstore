using System;
using System.Collections.Generic;

namespace DependencyStore.Domain
{
  public class LocalWarehouse
  {
    private DependencyCollection _dependencyCollection;

    public DependencyCollection DependencyCollection
    {
      get { return _dependencyCollection; }
      set { _dependencyCollection = value; }
    }
  }
}
