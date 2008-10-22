using System;
using System.Collections.Generic;
using System.IO;

namespace Machine.Partstore.Domain.FileSystem
{
  public class FileSystemFile : FileSystemEntry
  {
    private readonly DateTime _createdAt;
    private readonly DateTime _accessedAt;
    private readonly DateTime _modifiedAt;
    private readonly long _length;

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

    protected FileSystemFile()
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
      get { return _length; }
    }

    public override string ToString()
    {
      return String.Format(@"File<{0}>", this.Path);
    }
  }
}
