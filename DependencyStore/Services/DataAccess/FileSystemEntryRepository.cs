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
        return CreateFile(path.Full);
      }
      if (_fileSystem.IsDirectory(path.Full))
      {
        return CreateDirectory(path.Full, rules);
      }
      return null;
    }
    #endregion

    private FileSystemDirectory CreateDirectory(string path, FileAndDirectoryRules rules)
    {
      List<FileSystemEntry> entries = new List<FileSystemEntry>();
      foreach (string entryPath in _fileSystem.GetDirectories(path))
      {
        if (rules.IncludesDirectory(new FileSystemPath(entryPath)) != IncludeExclude.Exclude)
        {
          entries.Add(CreateDirectory(entryPath, rules));
        }
      }
      foreach (string entryPath in _fileSystem.GetFiles(path))
      {
        if (rules.IncludesFile(new FileSystemPath(entryPath)) != IncludeExclude.Exclude)
        {
          entries.Add(CreateFile(entryPath));
        }
      }
      FileSystemDirectory entry = new FileSystemDirectory();
      entry.Path = new FileSystemPath(path);
      entry.Entries = entries;
      return entry;
    }

    private FileSystemFile CreateFile(string path)
    {
      FileProperties properties = _fileSystem.GetFileProperties(path);
      FileSystemFile entry = new FileSystemFile();
      entry.Path = new FileSystemPath(path);
      entry.Length = properties.Length;
      entry.CreatedAt = properties.CreationTime;
      entry.ModifiedAt = properties.LastWriteTime;
      entry.AccessedAt = properties.LastAccessTime;
      return entry;
    }
  }
}
