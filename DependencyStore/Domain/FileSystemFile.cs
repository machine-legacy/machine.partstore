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

    public string Extension
    {
      get { return System.IO.Path.GetExtension(this.Path.Full); }
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
      return String.Format(@"File<{0}>", this.Path);
    }
  }
}
