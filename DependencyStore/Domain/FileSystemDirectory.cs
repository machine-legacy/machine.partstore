using System;
using System.Collections.Generic;
using System.IO;

using Machine.Container;

namespace DependencyStore.Domain
{
  public class FileSystemDirectory : FileSystemEntry
  {
    private readonly List<FileSystemEntry> _entries = new List<FileSystemEntry>();

    public List<FileSystemEntry> Entries
    {
      get { return _entries; }
    }

    public string Name
    {
      get { return this.Path.Name; }
    }

    public override IEnumerable<FileSystemEntry> Children
    {
      get
      {
        foreach (FileSystemEntry entry in _entries)
        {
          yield return entry;
        }
      }
    }

    public override IEnumerable<FileSystemEntry> BreadthFirstTree
    {
      get
      {
        yield return this;
        foreach (FileSystemEntry entry in this.Children)
        {
          foreach (FileSystemEntry child in entry.BreadthFirstTree)
          {
            yield return child;
          }
        }
      }
    }

    public override IEnumerable<FileSystemFile> BreadthFirstFiles
    {
      get
      {
        foreach (FileSystemEntry entry in this.Children)
        {
          foreach (FileSystemFile child in entry.BreadthFirstFiles)
          {
            yield return child;
          }
        }
      }
    }

    public FileSystemDirectory()
    {
    }

    public FileSystemDirectory(Purl path) 
     : base(path)
    {
    }

    public override string ToString()
    {
      return String.Format(@"Directory<{0}, {1}>", this.Path, this.Entries.Count);
    }

    public override Stream OpenForReading()
    {
      throw new YouFoundABugException();
    }

    public override long LengthInBytes
    {
      get { throw new YouFoundABugException(); }
    }
  }
}