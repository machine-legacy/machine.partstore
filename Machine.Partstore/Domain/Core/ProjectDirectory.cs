using System;
using System.Collections.Generic;

using Machine.Partstore.Domain.FileSystem;

namespace Machine.Partstore.Domain.Core
{
  public class ProjectDirectory
  {
    public static readonly ProjectDirectory Missing = new ProjectDirectory(Purl.Null);
    private readonly Purl _path;
    private FileSet _fileSet;

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

    public bool IsEmpty
    {
      get { return ToFileSet().IsEmpty; }
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
      {
        if (_fileSet == null)
        {
          _fileSet = FileSetFactory.CreateFileSetFrom(this.Path);
        }
        return _fileSet;
      }
    }

    public ProjectDirectory(Purl path)
    {
      _path = path;
    }
  }
}
