using System;
using System.Collections.Generic;
using System.IO;

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
    }

    public DateTime AccessedAt
    {
      get { return _accessedAt; }
    }

    public override DateTime ModifiedAt
    {
      get { return _modifiedAt; }
    }

    public string Name
    {
      get { return this.Path.Name; }
    }

    public override IEnumerable<FileSystemFile> BreadthFirstFiles
    {
      get { yield return this; }
    }

    public FileSystemFile()
    {
    }

    public FileSystemFile(Purl path)
     : base(path)
    {
    }

    public FileSystemFile(Purl path, long length, DateTime createdAt, DateTime accessedAt, DateTime modifiedAt)
     : base(path)
    {
      _length = length;
      _createdAt = createdAt;
      _accessedAt = accessedAt;
      _modifiedAt = modifiedAt;
    }

    public override Stream OpenForReading()
    {
      return File.OpenRead(this.Path.AsString);
    }

    public override long LengthInBytes
    {
      get { return this.Length; }
    }

    public override string ToString()
    {
      return String.Format(@"File<{0}>", this.Path);
    }
  }
}
