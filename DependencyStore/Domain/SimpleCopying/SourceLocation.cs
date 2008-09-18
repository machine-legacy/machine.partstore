using System;
using System.Collections.Generic;

using DependencyStore.Domain.Core;

namespace DependencyStore.Domain.SimpleCopying
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