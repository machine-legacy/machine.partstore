using System;
using System.Collections.Generic;

using DependencyStore.Domain;
using DependencyStore.Services.DataAccess;

namespace DependencyStore.Services
{
  public class Controller : IController
  {
    private readonly IFileAndDirectoryRulesRepository _fileAndDirectoryRulesRepository;
    private readonly IFileSystemEntryRepository _fileSystemEntryRepository;
    private readonly ILocationRepository _locationRepository;

    public Controller(IFileSystemEntryRepository fileSystemEntryRepository, ILocationRepository locationRepository, IFileAndDirectoryRulesRepository fileAndDirectoryRulesRepository)
    {
      _fileSystemEntryRepository = fileSystemEntryRepository;
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
      }
      foreach (Location location in _locationRepository.FindAllSinks())
      {
        FileSystemEntry entry = _fileSystemEntryRepository.FindEntry(location.Path, rules);
        if (entry != null)
        {
          foreach (FileSystemFile child in entry.BreadthFirstFiles)
          {
            FileSystemFile existing = latest.FindExistingByName(child);
            if (existing != null && child.IsOlderThan(existing))
            {
              Console.WriteLine("Replace: {0} ({1} vs {2})", child, child.ModifiedAt, existing.ModifiedAt);
            }
          }
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
