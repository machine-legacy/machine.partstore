using System;
using System.Collections.Generic;

using DependencyStore.Domain.Core;

namespace DependencyStore.Domain.Archiving
{
  public abstract class ArchivedFile : FileAsset
  {
    private readonly Purl _path;

    public override Core.Purl Purl
    {
      get { return _path; }
    }

    protected ArchivedFile(Core.Purl path)
    {
      _path = path;
    }
  }
}
