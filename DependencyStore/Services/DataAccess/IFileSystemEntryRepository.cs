using System;
using System.Collections.Generic;

using DependencyStore.Domain;

namespace DependencyStore.Services.DataAccess
{
  public interface IFileSystemEntryRepository
  {
    FileSystemEntry FindEntry(FileSystemPath path, FileAndDirectoryRules rules);
  }
}
