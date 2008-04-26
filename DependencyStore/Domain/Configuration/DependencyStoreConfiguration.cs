using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace DependencyStore.Domain.Configuration
{
  [XmlRoot("DependencyStore")]
  public class DependencyStoreConfiguration
  {
    private readonly List<BuildDirectoryConfiguration> _buildDirectories = new List<BuildDirectoryConfiguration>();
    private readonly List<LibraryDirectoryConfiguration> _libraryDirectories = new List<LibraryDirectoryConfiguration>();

    public List<BuildDirectoryConfiguration> BuildDirectories
    {
      get { return _buildDirectories; }
    }

    public List<LibraryDirectoryConfiguration> LibraryDirectories
    {
      get { return _libraryDirectories; }
    }
  }
}
