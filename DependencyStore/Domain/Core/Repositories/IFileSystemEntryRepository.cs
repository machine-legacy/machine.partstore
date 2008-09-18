using System;
using System.Collections.Generic;

using DependencyStore.Domain.Configuration;

namespace DependencyStore.Domain.Core.Repositories
{
  public interface IFileSystemEntryRepository
  {
    FileSystemEntry FindEntry(Purl path, FileAndDirectoryRules rules);
    FileSystemEntry FindEntry(Purl path);
  }
}
