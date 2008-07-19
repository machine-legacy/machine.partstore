using System;
using System.Collections.Generic;
using System.IO;

namespace DependencyStore.Domain
{
  public abstract class FileAsset
  {
    public abstract Purl Purl
    {
      get;
    }

    public abstract Stream OpenForReading();

    public abstract long LengthInBytes
    {
      get;
    }
  }
}
