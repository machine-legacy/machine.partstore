using System;
using System.Collections.Generic;

namespace DependencyStore.Domain
{
  public class BuildDirectoryPath : FileSystemPath
  {
    public BuildDirectoryPath(string uri)
      : base(uri)
    {
    }
  }
}