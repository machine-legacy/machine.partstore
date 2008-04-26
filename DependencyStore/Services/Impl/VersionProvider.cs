using System;
using System.Collections.Generic;
using DependencyStore.Domain;

namespace DependencyStore.Services.Impl
{
  public class VersionProvider : IVersionProvider
  {
    public void FindCommonVersion(IEnumerable<FileSystemFile> files)
    {
      foreach (FileSystemFile file in files)
      {
      }
    }
  }
}
