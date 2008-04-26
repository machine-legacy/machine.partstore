using System;
using System.Collections.Generic;

using DependencyStore.Domain;
using DependencyStore.Domain.Configuration;
using Machine.Core.Utility;

namespace DependencyStore.Services.DataAccess
{
  public class LocationRepository : ILocationRepository
  {
    private readonly IConfigurationRepository _configurationRepository;
    private readonly IFileSystemEntryRepository _fileSystemEntryRepository;

    public LocationRepository(IConfigurationRepository configurationRepository, IFileSystemEntryRepository fileSystemEntryRepository)
    {
      _configurationRepository = configurationRepository;
      _fileSystemEntryRepository = fileSystemEntryRepository;
    }

    #region ILocationRepository Members
    public IList<Location> FindAll(FileAndDirectoryRules rules)
    {
      DependencyStoreConfiguration configuration = _configurationRepository.FindConfiguration();
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

    public IList<SourceLocation> FindAllSources(FileAndDirectoryRules rules)
    {
      List<SourceLocation> locations = new List<SourceLocation>();
      foreach (Location location in FindAll(rules))
      {
        if (location.IsSource) locations.Add((SourceLocation)location);
      }
      return locations;
    }

    public IList<SinkLocation> FindAllSinks(FileAndDirectoryRules rules)
    {
      List<SinkLocation> locations = new List<SinkLocation>();
      foreach (Location location in FindAll(rules))
      {
        if (location.IsSink) locations.Add((SinkLocation)location);
      }
      return locations;
    }
    #endregion
  }
}
