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

    public LocationRepository(IConfigurationRepository configurationRepository)
    {
      _configurationRepository = configurationRepository;
    }

    #region ILocationRepository Members
    public IList<Location> FindAll()
    {
      DependencyStoreConfiguration configuration = _configurationRepository.FindConfiguration();
      List<Location> locations = new List<Location>();
      foreach (BuildDirectoryConfiguration build in configuration.BuildDirectories)
      {
        locations.Add(new BuildLocation(new FileSystemPath(build.Path)));
      }
      foreach (LibraryDirectoryConfiguration library in configuration.LibraryDirectories)
      {
        locations.Add(new LibraryLocation(new FileSystemPath(library.Path)));
      }
      return locations;
      return new Location[] {
        new BuildLocation(new FileSystemPath(@"C:\Home\Source\Machine\Build")),
        new LibraryLocation(new FileSystemPath(@"C:\Home\Source\JL\DependencyStore\Libraries")),
        new BuildLocation(new FileSystemPath(@"D:\Home\Source\Machine\Build")),
        new LibraryLocation(new FileSystemPath(@"D:\Home\Source\JL\DependencyStore\Libraries")),
        new BuildLocation(new FileSystemPath(@"D:\Home\Source\Projects\Whiteboard\Build")),
        new LibraryLocation(new FileSystemPath(@"D:\Home\Source\Projects\Whiteboard\Libraries")),
      };
    }

    public IList<Location> FindAllSources()
    {
      List<Location> locations = new List<Location>();
      foreach (Location location in FindAll())
      {
        if (location.IsSource) locations.Add(location);
      }
      return locations;
    }

    public IList<Location> FindAllSinks()
    {
      List<Location> locations = new List<Location>();
      foreach (Location location in FindAll())
      {
        if (location.IsSink) locations.Add(location);
      }
      return locations;
    }
    #endregion
  }
}
