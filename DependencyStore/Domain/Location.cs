using System;
using System.Collections.Generic;

namespace DependencyStore.Domain
{
  public abstract class Location
  {
    private FileSystemPath _path;

    public FileSystemPath Path
    {
      get { return _path; }
      set { _path = value; }
    }

    public abstract bool IsSource
    {
      get;
    }

    public bool IsSink
    {
      get { return !this.IsSource; }
    }

    protected Location()
    {
    }

    protected Location(FileSystemPath path)
    {
      _path = path;
    }
  }
}
