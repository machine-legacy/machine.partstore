using System;
using System.Collections.Generic;

using Machine.Core.Services;

using DependencyStore.Domain.Configuration;

namespace DependencyStore.Domain.FileSystem.Repositories.Impl
{
  public class FileSystemEntryRepository : IFileSystemEntryRepository
  {
    private readonly IFileSystem _fileSystem;
    private readonly IFileAndDirectoryRulesRepository _fileAndDirectoryRulesRepository;

    public FileSystemEntryRepository(IFileSystem fileSystem, IFileAndDirectoryRulesRepository fileAndDirectoryRulesRepository)
    {
      _fileSystem = fileSystem;
      _fileAndDirectoryRulesRepository = fileAndDirectoryRulesRepository;
    }

    #region IFileSystemEntryRepository Members
    public FileSystemEntry FindEntry(Purl path)
    {
      return FindEntry(path, _fileAndDirectoryRulesRepository.FindDefault());
    }

    public FileSystemEntry FindEntry(Purl path, FileAndDirectoryRules rules)
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

    private FileSystemDirectory CreateDirectory(Purl path, FileAndDirectoryRules rules)
    {
      FileSystemDirectory entry = new FileSystemDirectory(path);
      foreach (string subPath in _fileSystem.GetEntries(path.AsString))
      {
        Purl entryPath = new Purl(subPath);
        FileSystemEntry subEntry = FindEntry(entryPath, rules);
        if (subEntry != null)
        {
          entry.Entries.Add(subEntry);
        }
      }
      return entry;
    }

    private static FileSystemFile CreateFile(Purl path)
    {
      return FileSystemFileFactory.CreateFile(path);
    }
  }
  public static class FileSystemFileFactory
  {
    public static FileSystemFile CreateFile(Purl path)
    {
      FileProperties properties = Infrastructure.FileSystem.GetFileProperties(path.AsString);
      return new FileSystemFile(path, properties.Length, properties.CreationTime, properties.LastAccessTime, properties.LastWriteTime);
    }
  }
}
