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

    public abstract DateTime ModifiedAt
    {
      get;
    }

    public bool IsNewerThan(FileAsset file)
    {
      return this.ModifiedAt > file.ModifiedAt;
    }

    public bool IsOlderThan(FileAsset file)
    {
      return this.ModifiedAt < file.ModifiedAt;
    }

    public bool IsSameAgeAs(FileAsset file)
    {
      return this.ModifiedAt == file.ModifiedAt;
    }
  }
}
