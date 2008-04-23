using System;
using System.Collections.Generic;

namespace DependencyStore.Domain
{
  public class BuildLocation : Location
  {
    public override bool IsSource
    {
      get { return true; }
    }

    public BuildLocation(FileSystemPath path)
      : base(path)
    {
    }

    public BuildLocation()
    {
    }
  }
}