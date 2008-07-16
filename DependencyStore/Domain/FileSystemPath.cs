using System;
using System.Collections.Generic;
using System.IO;
using DependencyStore.Utility;

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

    public FileSystemPath Chroot(FileSystemPath root)
    {
      if (!this.Full.StartsWith(root.Full))
      {
        throw new InvalidOperationException(String.Format("Can't chroot {0} with {1}", this, root));
      }
      string rootPath = PathHelper.NormalizeDirectorySlashes(root.Full);
      return new FileSystemPath(this.Full.Substring(rootPath.Length));
    }

    public override bool Equals(object obj)
    {
      if (obj is FileSystemPath)
      {
        return ((FileSystemPath)obj).Full.Equals(_full, StringComparison.InvariantCultureIgnoreCase);
      }
      return base.Equals(obj);
    }

    public override Int32 GetHashCode()
    {
      return this.Full.GetHashCode();
    }

    public override string ToString()
    {
      return String.Format("Path<{0}>", this.Full);
    }

    public FileSystemPath Join(string path)
    {
      return new FileSystemPath(Path.Combine(_full, path));
    }

    public FileSystemPath Join(FileSystemPath path)
    {
      return Join(path.Full);
    }
  }
}
