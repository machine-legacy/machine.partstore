using System;
using System.Collections.Generic;

namespace DependencyStore.Domain
{
  public class FileSystemEntry
  {
    private FileSystemPath _path;

    public FileSystemPath Path
    {
      get { return _path; }
      set { _path = value; }
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

    public FileSystemEntry()
    {
    }

    public FileSystemEntry(FileSystemPath path)
    {
      _path = path;
    }
  }
}