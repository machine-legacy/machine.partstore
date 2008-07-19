using System;
using System.Collections.Generic;

namespace DependencyStore.Domain
{
  public class SourceLocation : Location
  {
    public override bool IsSource
    {
      get { return true; }
    }

    public SourceLocation(Purl path, FileSystemEntry entry)
      : base(path, entry)
    {
    }

    public SourceLocation()
    {
    }
  }
}