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
  }
}