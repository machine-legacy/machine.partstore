using System;
using System.Collections.Generic;

using DependencyStore.Domain.Core;
using DependencyStore.Domain.Configuration;

namespace DependencyStore.Services.DataAccess.Impl
{
  public class LocationRepository : ILocationRepository
  {
    private readonly ICurrentConfiguration _currentConfiguration;
    private readonly IFileSystemEntryRepository _fileSystemEntryRepository;

    public LocationRepository(ICurrentConfiguration currentConfiguration, IFileSystemEntryRepository fileSystemEntryRepository)
    {
      _currentConfiguration = currentConfiguration;
      _fileSystemEntryRepository = fileSystemEntryRepository;
    }

    #region ILocationRepository Members
    private IList<Location> FindAll(DependencyStoreConfiguration configuration)
    {
      List<Location> locations = new List<Location>();
      foreach (BuildDirectoryConfiguration build in configuration.BuildDirectories)
      {
        Purl path = new Purl(build.Path);
        FileSystemEntry fileSystemEntry = _fileSystemEntryRepository.FindEntry(path, configuration.FileAndDirectoryRules);
        if (fileSystemEntry != null)
        {
          locations.Add(new SourceLocation(path, fileSystemEntry));
        }
        else
        {
          DomainEvents.OnLocationNotFound(this, new LocationNotFoundEventArgs(build.AsPurl));
        }
      }
      foreach (LibraryDirectoryConfiguration library in configuration.LibraryDirectories)
      {
        Purl path = new Purl(library.Path);
        FileSystemEntry fileSystemEntry = _fileSystemEntryRepository.FindEntry(path, configuration.FileAndDirectoryRules);
        if (fileSystemEntry != null)
        {
          locations.Add(new SinkLocation(path, fileSystemEntry));
        }
        else
        {
          DomainEvents.OnLocationNotFound(this, new LocationNotFoundEventArgs(library.AsPurl));
        }
      }
      return locations;
    }

    public IList<SourceLocation> FindAllSources()
    {
      List<SourceLocation> locations = new List<SourceLocation>();
      foreach (Location location in FindAll(_currentConfiguration.DefaultConfiguration))
      {
        if (location.IsSource) locations.Add((SourceLocation)location);
      }
      return locations;
    }

    public IList<SinkLocation> FindAllSinks()
    {
      List<SinkLocation> locations = new List<SinkLocation>();
      foreach (Location location in FindAll(_currentConfiguration.DefaultConfiguration))
      {
        if (location.IsSink) locations.Add((SinkLocation)location);
      }
      return locations;
    }
    #endregion
  }
}
