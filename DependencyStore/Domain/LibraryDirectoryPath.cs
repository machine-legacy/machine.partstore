using System;
using System.Collections.Generic;

namespace DependencyStore.Domain
{
  public class LibraryDirectoryPath : FileSystemPath
  {
    public LibraryDirectoryPath(string uri)
      : base(uri)
    {
    }
  }
}