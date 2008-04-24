using System;
using System.Collections.Generic;

using DependencyStore.Domain;
using DependencyStore.Services.DataAccess;

using Machine.Core.Utility;
using Machine.Core.Services;

namespace DependencyStore.Services
{
  public class Controller : IController
  {
    private readonly IFileAndDirectoryRulesRepository _fileAndDirectoryRulesRepository;
    private readonly IFileSystemEntryRepository _fileSystemEntryRepository;
    private readonly ILocationRepository _locationRepository;
    private readonly IFileSystem _fileSystem;

    public Controller(IFileSystemEntryRepository fileSystemEntryRepository, ILocationRepository locationRepository, IFileAndDirectoryRulesRepository fileAndDirectoryRulesRepository, IFileSystem fileSystem)
    {
      _fileSystemEntryRepository = fileSystemEntryRepository;
      _fileSystem = fileSystem;
      _fileAndDirectoryRulesRepository = fileAndDirectoryRulesRepository;
      _locationRepository = locationRepository;
    }

    #region IController Members
    public void Show()
    {
      LatestFiles latest = new LatestFiles();
      FileAndDirectoryRules rules = _fileAndDirectoryRulesRepository.FindDefault();
      foreach (Location location in _locationRepository.FindAllSources())
      {
        FileSystemEntry entry = _fileSystemEntryRepository.FindEntry(location.Path, rules);
        if (entry != null)
        {
          latest.Add(entry.BreadthFirstFiles);
        }
        else
        {
          Console.WriteLine("Missing: {0}", location.Path);  
        }
      }
      foreach (Location location in _locationRepository.FindAllSinks())
      {
        Console.WriteLine("Under: {0}", location.Path);
        FileSystemEntry entry = _fileSystemEntryRepository.FindEntry(location.Path, rules);
        if (entry != null)
        {
          foreach (FileSystemFile child in entry.BreadthFirstFiles)
          {
            FileSystemFile existing = latest.FindExistingByName(child);
            if (existing != null && child.IsOlderThan(existing))
            {
              TimeSpan age = existing.ModifiedAt - child.ModifiedAt;
              FileSystemPath chrooted = child.Path.Chroot(location.Path);
              Console.WriteLine("+ {0} {1}", chrooted.Full, TimeSpanHelper.ToPrettyString(age));
              // _fileSystem.CopyFile(existing.Path.Full, child.Path.Full, true);
            }
          }
        }
        else
        {
          Console.WriteLine("Missing: {0}", location.Path);  
        }
      }
    }

    public void Clear()
    {
    }

    public void Refresh()
    {
    }
    #endregion
  }
}
