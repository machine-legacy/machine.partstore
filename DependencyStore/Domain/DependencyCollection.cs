using System;
using System.Collections.Generic;

namespace DependencyStore.Domain
{
  public class DependencyCollection
  {
    private readonly List<Dependency> _dependencies = new List<Dependency>();
    private DateTime _lastUpdated;

    public List<Dependency> Dependencies
    {
      get { return _dependencies; }
    }

    public DateTime LastUpdated
    {
      get { return _lastUpdated; }
      set { _lastUpdated = value; }
    }
  }
}
