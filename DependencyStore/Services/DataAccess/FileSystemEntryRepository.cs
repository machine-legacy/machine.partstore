using System;
using System.Collections.Generic;

using Machine.Core.Services;

using DependencyStore.Domain;

namespace DependencyStore.Services.DataAccess
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
      if (_fileSystem.IsFile(path.Full))
      {
        if (rules.IncludesFile(path) != IncludeExclude.Exclude)
        {
          return CreateFile(path);
        }
      }
      else if (_fileSystem.IsDirectory(path.Full))
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
      List<FileSystemEntry> entries = new List<FileSystemEntry>();
      foreach (string subPath in _fileSystem.GetEntries(path.Full))
      {
        FileSystemPath entryPath = new FileSystemPath(subPath);
        FileSystemEntry subEntry = FindEntry(entryPath, rules);
        if (subEntry != null)
        {
          entries.Add(subEntry);
        }
      }
      FileSystemDirectory entry = new FileSystemDirectory();
      entry.Path = path;
      entry.Entries = entries;
      return entry;
    }

    private FileSystemFile CreateFile(FileSystemPath path)
    {
      FileProperties properties = _fileSystem.GetFileProperties(path.Full);
      FileSystemFile entry = new FileSystemFile();
      entry.Path = path;
      entry.Length = properties.Length;
      entry.CreatedAt = properties.CreationTime;
      entry.ModifiedAt = properties.LastWriteTime;
      entry.AccessedAt = properties.LastAccessTime;
      return entry;
    }
  }
}
