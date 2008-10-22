using System;
using System.Collections.Generic;

using DependencyStore.Domain.Configuration;

namespace DependencyStore.Domain.FileSystem.Repositories
{
  public interface IFileSystemEntryRepository
  {
    FileSystemEntry FindEntry(Purl path, FileAndDirectoryRules rules);
    FileSystemEntry FindEntry(Purl path);
  }
}
