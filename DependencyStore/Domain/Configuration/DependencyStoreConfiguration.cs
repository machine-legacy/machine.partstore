using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using DependencyStore.Domain.Core;

namespace DependencyStore.Domain.Configuration
{
  [XmlRoot("DependencyStore")]
  public class DependencyStoreConfiguration
  {
    private readonly List<BuildDirectoryConfiguration> _buildDirectories = new List<BuildDirectoryConfiguration>();
    private readonly List<LibraryDirectoryConfiguration> _libraryDirectories = new List<LibraryDirectoryConfiguration>();
    private readonly List<ProjectConfiguration> _projectConfigurations = new List<ProjectConfiguration>();
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

    public List<ProjectConfiguration> ProjectConfigurations
    {
      get { return _projectConfigurations; }
    }

    [XmlIgnore]
    public FileAndDirectoryRules FileAndDirectoryRules
    {
      get { return _fileAndDirectoryRules; }
      set { _fileAndDirectoryRules = value; }
    }

    public List<BuildDirectoryConfiguration> BuildDirectories
    {
      get { return _buildDirectories; }
    }

    public List<LibraryDirectoryConfiguration> LibraryDirectories
    {
      get { return _libraryDirectories; }
    }

    public virtual void EnsureValid()
    {
      if (String.IsNullOrEmpty(_repositoryDirectory))
      {
        throw new ConfigurationException("Invalid Repository Directory!");
      }
      foreach (ProjectConfiguration configuration in _projectConfigurations)
      {
        configuration.EnsureValid();
      }
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
