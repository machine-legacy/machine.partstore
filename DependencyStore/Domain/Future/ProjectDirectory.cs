using System;
using System.Collections.Generic;

namespace DependencyStore.Domain.Future
{
  public class ProjectDirectory
  {
    private readonly List<Dependency> _dependencies = new List<Dependency>();

    public List<Dependency> Dependencies
    {
      get { return _dependencies; }
    }
  }
}
