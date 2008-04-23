using System;
using System.Collections.Generic;

namespace DependencyStore.Domain
{
  public class FileSystemDirectory : FileSystemEntry
  {
    private List<FileSystemEntry> _entries = new List<FileSystemEntry>();

    public List<FileSystemEntry> Entries
    {
      get { return _entries; }
      set { _entries = value; }
    }

    public string Name
    {
      get { return this.Path.Name; }
    }

    public override string ToString()
    {
      return String.Format(@"Directory<{0}, {1}>", this.Path, this.Entries.Count);
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

    public override IEnumerable<FileSystemEntry> DepthFirstTree
    {
      get
      {
        foreach (FileSystemEntry entry in this.Children)
        {
          foreach (FileSystemEntry child in entry.DepthFirstTree)
          {
            yield return child;
          }
        }
        yield return this;
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
  }
}