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

    public string Directory
    {
      get { return System.IO.Path.GetDirectoryName((this.Path.Full)); }
    }

    public string Name
    {
      get { return System.IO.Path.GetFileName((this.Path.Full)); }
    }

    public override string ToString()
    {
      return String.Format(@"Directory<{0}, {1}>", this.Path, this.Entries.Count);
    }
  }
}