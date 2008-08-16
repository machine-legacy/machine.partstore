using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace DependencyStore.Domain.Configuration
{
  [XmlRoot("DependencyStore")]
  public class SimpleDependencyStoreConfiguration
  {
    private FileAndDirectoryRules _fileAndDirectoryRules;
    private string _repositoryDirectory;

    [XmlAttribute]
    public string Repository
    {
      get { return _repositoryDirectory; }
      set { _repositoryDirectory = value; }
    }

    [XmlIgnore]
    public Purl RepositoryDirectory
    {
      get { return new Purl(_repositoryDirectory); }
    }

    [XmlIgnore]
    public FileAndDirectoryRules FileAndDirectoryRules
    {
      get { return _fileAndDirectoryRules; }
      set { _fileAndDirectoryRules = value; }
    }

    public virtual void EnsureValid()
    {
      if (String.IsNullOrEmpty(_repositoryDirectory))
      {
        throw new ConfigurationException("Invalid Repository Directory!");
      }
    }
  }
  [XmlRoot("DependencyStore")]
  public class DependencyStoreConfiguration : SimpleDependencyStoreConfiguration
  {
    private readonly List<BuildDirectoryConfiguration> _buildDirectories = new List<BuildDirectoryConfiguration>();
    private readonly List<LibraryDirectoryConfiguration> _libraryDirectories = new List<LibraryDirectoryConfiguration>();
    private readonly List<ProjectConfiguration> _projectConfigurations = new List<ProjectConfiguration>();

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

    public override void EnsureValid()
    {
      base.EnsureValid();
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
