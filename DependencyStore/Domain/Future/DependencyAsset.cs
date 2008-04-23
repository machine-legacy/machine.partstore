using System;
using System.Collections.Generic;

namespace DependencyStore.Domain.Future
{
  public class DependencyAsset
  {
    private string _name;
    private Version _version;

    public string Name
    {
      get { return _name; }
      set { _name = value; }
    }

    public Version Version
    {
      get { return _version; }
      set { _version = value; }
    }
  }
}
