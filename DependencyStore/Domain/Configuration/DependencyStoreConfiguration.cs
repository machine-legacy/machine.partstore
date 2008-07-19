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
    private readonly List<ProjectConfiguration> _projectConfigurations = new List<ProjectConfiguration>();
    private FileAndDirectoryRules _fileAndDirectoryRules;
    private string _packageDirectory;

    public List<BuildDirectoryConfiguration> BuildDirectories
    {
      get { return _buildDirectories; }
    }

    public List<LibraryDirectoryConfiguration> LibraryDirectories
    {
      get { return _libraryDirectories; }
    }

    public List<ProjectConfiguration> ProjectConfigurations
    {
      get { return _projectConfigurations; }
    }

    [XmlAttribute]
    public string Packages
    {
      get { return _packageDirectory; }
      set { _packageDirectory = value; }
    }

    [XmlIgnore]
    public Purl PackageDirectory
    {
      get { return new Purl(_packageDirectory); }
    }

    [XmlIgnore]
    public FileAndDirectoryRules FileAndDirectoryRules
    {
      get { return _fileAndDirectoryRules; }
      set { _fileAndDirectoryRules = value; }
    }

    public void EnsureValid()
    {
      if (String.IsNullOrEmpty(_packageDirectory))
      {
        throw new ConfigurationException("Invalid Package Directory!");
      }
      foreach (BuildDirectoryConfiguration configuration in _buildDirectories)
      {
        configuration.EnsureValid();
      }
      foreach (LibraryDirectoryConfiguration configuration in _libraryDirectories)
      {
        configuration.EnsureValid();
      }
      foreach (ProjectConfiguration configuration in _projectConfigurations)
      {
        configuration.EnsureValid();
      }
    }
  }
}
