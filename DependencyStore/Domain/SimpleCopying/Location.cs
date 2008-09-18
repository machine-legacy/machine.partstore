using System;
using System.Collections.Generic;

using DependencyStore.Domain.Core;

namespace DependencyStore.Domain.SimpleCopying
{
  public abstract class Location
  {
    private Purl _path;
    private FileSystemEntry _fileEntry;

    public Purl Path
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

    protected Location(Purl path, FileSystemEntry fileEntry)
    {
      _path = path;
      _fileEntry = fileEntry;
    }

    public FileSet ToFileSet()
    {
      FileSet fileSet = new FileSet();
      AddTo(fileSet);
      return fileSet;
    }

    public void AddTo(FileSet fileSet)
    {
      fileSet.AddAll(this.FileEntry.BreadthFirstFiles);
    }

    public bool HasFile(FileAsset file)
    {
      return ToFileSet().Contains(file);
    }

    public override string ToString()
    {
      return "Location<" + this.Path + ">";
    }
  }
}
