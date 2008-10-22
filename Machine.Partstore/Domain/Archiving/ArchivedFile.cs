using System;
using System.Collections.Generic;

using Machine.Partstore.Domain.FileSystem;

namespace Machine.Partstore.Domain.Archiving
{
  public abstract class ArchivedFile : FileAsset
  {
    private readonly Purl _path;

    public override Purl Purl
    {
      get { return _path; }
    }

    protected ArchivedFile(Purl path)
    {
      _path = path;
    }
  }
}
