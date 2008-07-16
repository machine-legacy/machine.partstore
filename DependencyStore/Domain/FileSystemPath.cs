using System;
using System.Collections.Generic;
using System.IO;

using DependencyStore.Utility;

namespace DependencyStore.Domain
{
  public class FileSystemPath
  {
    private readonly string _path;

    public string AsString
    {
      get { return _path; }
    }

    public string Name
    {
      get { return Path.GetFileName(_path); }
    }

    public string Directory
    {
      get { return Path.GetDirectoryName(_path); }
    }

    public FileSystemPath(string uri)
    {
      _path = uri;
    }

    public bool IsSubPath(FileSystemPath subPath)
    {
      return this.AsString.StartsWith(subPath.AsString);
    }

    public FileSystemPath Chroot(FileSystemPath root)
    {
      if (!IsSubPath(root))
      {
        throw new InvalidOperationException(String.Format("Can't chroot {0} with {1}", this, root));
      }
      string rootPath = PathHelper.NormalizeDirectorySlashes(root.AsString);
      return new FileSystemPath(this.AsString.Substring(rootPath.Length));
    }

    public override bool Equals(object obj)
    {
      if (obj is FileSystemPath)
      {
        return ((FileSystemPath)obj).AsString.Equals(_path, StringComparison.InvariantCultureIgnoreCase);
      }
      return base.Equals(obj);
    }

    public override Int32 GetHashCode()
    {
      return this.AsString.GetHashCode();
    }

    public override string ToString()
    {
      return String.Format("Path<{0}>", this.AsString);
    }

    public FileSystemPath Join(string path)
    {
      return new FileSystemPath(Path.Combine(_path, path));
    }

    public FileSystemPath Join(FileSystemPath path)
    {
      return Join(path.AsString);
    }

    public Stream CreateFile()
    {
      return File.Create(this.AsString);
    }
  }
}
