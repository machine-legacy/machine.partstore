using System;
using System.Collections.Generic;

using DependencyStore.Domain.Configuration;
using DependencyStore.Domain.Core;

namespace DependencyStore.Domain.Core.Repositories
{
  public interface IFileSystemEntryRepository
  {
    FileSystemEntry FindEntry(Purl path, FileAndDirectoryRules rules);
  }
}
