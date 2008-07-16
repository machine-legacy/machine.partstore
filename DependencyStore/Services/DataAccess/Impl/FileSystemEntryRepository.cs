using System;
using System.Collections.Generic;

using Machine.Core.Services;

using DependencyStore.Domain;

namespace DependencyStore.Services.DataAccess.Impl
{
  public class FileSystemEntryRepository : IFileSystemEntryRepository
  {
    private readonly IFileSystem _fileSystem;

    public FileSystemEntryRepository(IFileSystem fileSystem)
    {
      _fileSystem = fileSystem;
    }

    #region IFileSystemEntryRepository Members
    public FileSystemEntry FindEntry(FileSystemPath path, FileAndDirectoryRules rules)
    {
      if (_fileSystem.IsFile(path.AsString))
      {
        if (rules.IncludesFile(path) != IncludeExclude.Exclude)
        {
          return CreateFile(path);
        }
      }
      else if (_fileSystem.IsDirectory(path.AsString))
      {
        if (rules.IncludesDirectory(path) != IncludeExclude.Exclude)
        {
          return CreateDirectory(path, rules);
        }
      }
      return null;
    }
    #endregion

    private FileSystemDirectory CreateDirectory(FileSystemPath path, FileAndDirectoryRules rules)
    {
      FileSystemDirectory entry = new FileSystemDirectory(path);
      foreach (string subPath in _fileSystem.GetEntries(path.AsString))
      {
        FileSystemPath entryPath = new FileSystemPath(subPath);
        FileSystemEntry subEntry = FindEntry(entryPath, rules);
        if (subEntry != null)
        {
          entry.Entries.Add(subEntry);
        }
      }
      return entry;
    }

    private FileSystemFile CreateFile(FileSystemPath path)
    {
      FileProperties properties = _fileSystem.GetFileProperties(path.AsString);
      FileSystemFile entry = new FileSystemFile(path);
      entry.Length = properties.Length;
      entry.CreatedAt = properties.CreationTime;
      entry.ModifiedAt = properties.LastWriteTime;
      entry.AccessedAt = properties.LastAccessTime;
      return entry;
    }
  }
}
