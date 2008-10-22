using System;
using System.Collections.Generic;

using Machine.Partstore.Domain.FileSystem;

namespace Machine.Partstore.Domain.Core
{
  public class ProjectDirectory
  {
    public static readonly ProjectDirectory Missing = new ProjectDirectory(Purl.Null);
    private readonly Purl _path;

    public bool IsMissing
    {
      get { return _path == Purl.Null; }
    }

    public Purl Path
    {
      get
      {
        if (IsMissing)
        {
          throw new InvalidOperationException("Directory is missing, unconfigured, or just plain obscure!");
        }
        return _path;
      }
    }

    public Purl GetRelativeTo(string relative)
    {
      return GetRelativeTo(new Purl(relative));
    }

    public Purl GetRelativeTo(Purl relative)
    {
      return this.Path.Join(relative);
    }

    public FileSet ToFileSet()
    {
      return FileSetFactory.CreateFileSetFrom(this.Path);
    }

    public ProjectDirectory(Purl path)
    {
      _path = path;
    }
  }
}
