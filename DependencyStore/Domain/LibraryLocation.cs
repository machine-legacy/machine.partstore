using System;
using System.Collections.Generic;

namespace DependencyStore.Domain
{
  public class LibraryLocation : Location
  {
    public override bool IsSource
    {
      get { return false; }
    }

    public LibraryLocation(FileSystemPath path)
      : base(path)
    {
    }

    public LibraryLocation()
    {
    }

    public void Update(LatestFiles files)
    {
    }
  }
}