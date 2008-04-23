using System;
using System.Collections.Generic;
using System.IO;

namespace DependencyStore.Domain
{
  public class FileSystemPath
  {
    private readonly string _full;

    public string Full
    {
      get { return _full; }
    }

    public string Name
    {
      get { return Path.GetFileName(_full); }
    }

    public string Directory
    {
      get { return Path.GetDirectoryName(_full); }
    }

    public FileSystemPath(string uri)
    {
      _full = uri;
    }

    public override bool Equals(object obj)
    {
      if (obj is FileSystemPath)
      {
        return ((FileSystemPath)obj).Full.Equals(_full, StringComparison.InvariantCultureIgnoreCase);
      }
      return base.Equals(obj);
    }

    public override int GetHashCode()
    {
      return this.Full.GetHashCode();
    }

    public override string ToString()
    {
      return String.Format("Path<{0}>", this.Full);
    }
  }
}
