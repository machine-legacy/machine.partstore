using System;
using System.Collections.Generic;
using System.IO;

using Machine.Partstore.Utility;

namespace Machine.Partstore.Domain.FileSystem
{
  /**
   * file:///c/Source/Project/File.cs
   * file:///c/Source/Project/File.zip/SomeFile.cs
   */
  public class Purl
  {
    private readonly string _path;
    public static readonly Purl Null = new Purl(String.Empty);

    public string AsString
    {
      get { return _path; }
    }

    public string Name
    {
      get { return Path.GetFileName(this.AsString); }
    }

    public string NameWithoutExtension
    {
      get { return Path.GetFileNameWithoutExtension(this.AsString); }
    }

    public string Directory
    {
      get { return Path.GetDirectoryName(this.AsString); }
    }

    public Purl Parent
    {
      get { return new Purl(this.Directory); }
    }

    public Purl(string path)
    {
      _path = path;
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
        return ((Purl)obj).AsString.Equals(this.AsString, StringComparison.InvariantCultureIgnoreCase);
      }
      return false;
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

    public void CreateParentDirectory()
    {
      System.IO.Directory.CreateDirectory(this.Directory);
    }

    public static Purl FindCommonDirectory(params Purl[] paths)
    {
      List<string> strings = new List<string>();
      foreach (Purl path in paths)
      {
        strings.Add(path.AsString);
      }
      string common = StringHelper.FindLongestCommonPrefix(strings);
      if (!System.IO.Directory.Exists(common))
      {
        common = Path.GetDirectoryName(common);
      }
      return new Purl(PathHelper.NormalizeDirectorySlashes(common));
    }

    public static Purl For(string path)
    {
      return new Purl(path);
    }
  }
}
