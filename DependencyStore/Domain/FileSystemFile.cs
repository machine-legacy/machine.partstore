using System;
using System.Collections.Generic;

namespace DependencyStore.Domain
{
  public class FileSystemFile : FileSystemEntry
  {
    private long _length;
    private DateTime _createdAt;
    private DateTime _accessedAt;
    private DateTime _modifiedAt;

    public long Length
    {
      get { return _length; }
      set { _length = value; }
    }

    public DateTime CreatedAt
    {
      get { return _createdAt; }
      set { _createdAt = value; }
    }

    public DateTime AccessedAt
    {
      get { return _accessedAt; }
      set { _accessedAt = value; }
    }

    public DateTime ModifiedAt
    {
      get { return _modifiedAt; }
      set { _modifiedAt = value; }
    }

    public string Name
    {
      get { return this.Path.Name; }
    }

    public override string ToString()
    {
      return String.Format(@"File<{0}>", this.Path);
    }

    public override IEnumerable<FileSystemFile> BreadthFirstFiles
    {
      get { yield return this; }
    }

    public bool IsNewerThan(FileSystemFile file)
    {
      return this.ModifiedAt > file.ModifiedAt;
    }

    public bool IsOlderThan(FileSystemFile file)
    {
      return this.ModifiedAt < file.ModifiedAt;
    }

    public bool IsSameAgeAs(FileSystemFile file)
    {
      return this.ModifiedAt == file.ModifiedAt;
    }

    public FileSystemFile()
    {
    }

    public FileSystemFile(FileSystemPath path)
     : base(path)
    {
    }
  }
}
