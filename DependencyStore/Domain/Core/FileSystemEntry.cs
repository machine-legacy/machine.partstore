using System;
using System.Collections.Generic;

namespace DependencyStore.Domain.Core
{
  public abstract class FileSystemEntry : FileAsset
  {
    private Purl _path;

    public Purl Path
    {
      get { return _path; }
      set { _path = value; }
    }

    public override Purl Purl
    {
      get { return _path; }
    }

    public virtual IEnumerable<FileSystemEntry> Children
    {
      get { yield break; }
    }

    public virtual IEnumerable<FileSystemEntry> BreadthFirstTree
    {
      get { yield return this; }
    }

    public virtual IEnumerable<FileSystemFile> BreadthFirstFiles
    {
      get { yield break; }
    }

    protected FileSystemEntry()
    {
    }

    protected FileSystemEntry(Purl path)
    {
      _path = path;
    }
  }
}