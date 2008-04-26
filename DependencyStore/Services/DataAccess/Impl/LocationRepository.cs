using System;
using System.Collections.Generic;

using DependencyStore.Domain;
using DependencyStore.Domain.Configuration;

namespace DependencyStore.Services.DataAccess.Impl
{
  public class LocationRepository : ILocationRepository
  {
    private readonly IFileSystemEntryRepository _fileSystemEntryRepository;

    public LocationRepository(IFileSystemEntryRepository fileSystemEntryRepository)
    {
      _fileSystemEntryRepository = fileSystemEntryRepository;
    }

    #region ILocationRepository Members
    private IList<Location> FindAll(DependencyStoreConfiguration configuration, FileAndDirectoryRules rules)
    {
      List<Location> locations = new List<Location>();
      foreach (BuildDirectoryConfiguration build in configuration.BuildDirectories)
      {
        FileSystemPath path = new FileSystemPath(build.Path);
        FileSystemEntry fileSystemEntry = _fileSystemEntryRepository.FindEntry(path, rules);
        if (fileSystemEntry != null)
        {
          locations.Add(new SourceLocation(path, fileSystemEntry));
        }
      }
      foreach (LibraryDirectoryConfiguration library in configuration.LibraryDirectories)
      {
        FileSystemPath path = new FileSystemPath(library.Path);
        FileSystemEntry fileSystemEntry = _fileSystemEntryRepository.FindEntry(path, rules);
        if (fileSystemEntry != null)
        {
          locations.Add(new SinkLocation(path, fileSystemEntry));
        }
      }
      return locations;
    }

    public IList<SourceLocation> FindAllSources(DependencyStoreConfiguration configuration, FileAndDirectoryRules rules)
    {
      List<SourceLocation> locations = new List<SourceLocation>();
      foreach (Location location in FindAll(configuration, rules))
      {
        if (location.IsSource) locations.Add((SourceLocation)location);
      }
      return locations;
    }

    public IList<SinkLocation> FindAllSinks(DependencyStoreConfiguration configuration, FileAndDirectoryRules rules)
    {
      List<SinkLocation> locations = new List<SinkLocation>();
      foreach (Location location in FindAll(configuration, rules))
      {
        if (location.IsSink) locations.Add((SinkLocation)location);
      }
      return locations;
    }
    #endregion
  }
}
