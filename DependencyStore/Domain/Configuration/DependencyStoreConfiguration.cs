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
    private FileAndDirectoryRules _fileAndDirectoryRules;
    private FileSystemPath _packageDirectory;

    public List<BuildDirectoryConfiguration> BuildDirectories
    {
      get { return _buildDirectories; }
    }

    public List<LibraryDirectoryConfiguration> LibraryDirectories
    {
      get { return _libraryDirectories; }
    }

    [XmlIgnore]
    public FileSystemPath PackageDirectory
    {
      get { return _packageDirectory; }
      set { _packageDirectory = value; }
    }

    [XmlIgnore]
    public FileAndDirectoryRules FileAndDirectoryRules
    {
      get { return _fileAndDirectoryRules; }
      set { _fileAndDirectoryRules = value; }
    }

    public void EnsureValid()
    {
      foreach (BuildDirectoryConfiguration configuration in _buildDirectories)
      {
        configuration.EnsureValid();
      }
      foreach (LibraryDirectoryConfiguration configuration in _libraryDirectories)
      {
        configuration.EnsureValid();
      }
    }
  }
}
