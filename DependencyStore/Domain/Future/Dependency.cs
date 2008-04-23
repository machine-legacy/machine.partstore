using System;
using System.Collections.Generic;

namespace DependencyStore.Domain.Future
{
  public class Dependency
  {
    private readonly List<DependencyAsset> _dependencies = new List<DependencyAsset>();
    private string _name;

    public string Name
    {
      get { return _name; }
      set { _name = value; }
    }

    public List<DependencyAsset> Dependencies
    {
      get { return _dependencies; }
    }
  }
}
