using System;
using System.Collections.Generic;

namespace DependencyStore.Domain
{
  public abstract class Location
  {
    private FileSystemPath _path;
    private FileSystemEntry _fileEntry;

    public FileSystemPath Path
    {
      get { return _path; }
      set { _path = value; }
    }

    public FileSystemEntry FileEntry
    {
      get { return _fileEntry; }
      set { _fileEntry = value; }
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

    protected Location(FileSystemPath path, FileSystemEntry fileEntry)
    {
      _path = path;
      _fileEntry = fileEntry;
    }

    public FileSet ToFileSet()
    {
      FileSet fileSet = new FileSet();
      fileSet.Add(this);
      return fileSet;
    }
  }
}
