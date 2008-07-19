using System;
using System.Collections.Generic;
using System.IO;

using DependencyStore.Utility;

namespace DependencyStore.Domain
{
  public class Purl
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

    public Purl(string uri)
    {
      _path = uri;
    }

    public bool IsARoot(Purl path)
    {
      return this.AsString.StartsWith(path.AsString);
    }

    public Purl ChangeRoot(Purl root)
    {
      if (!IsARoot(root))
      {
        throw new InvalidOperationException(String.Format("Unable to change root of {0} to {1}", this, root));
      }
      string rootPath = PathHelper.NormalizeDirectorySlashes(root.AsString);
      return new Purl(this.AsString.Substring(rootPath.Length));
    }

    public override bool Equals(object obj)
    {
      if (obj is Purl)
      {
        return ((Purl)obj).AsString.Equals(_path, StringComparison.InvariantCultureIgnoreCase);
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

    public Purl Join(string path)
    {
      return new Purl(Path.Combine(_path, path));
    }

    public Purl Join(Purl path)
    {
      return Join(path.AsString);
    }

    public Stream CreateFile()
    {
      return File.Create(this.AsString);
    }
  }
}
