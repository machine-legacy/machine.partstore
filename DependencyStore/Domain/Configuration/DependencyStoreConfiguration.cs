using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using DependencyStore.Domain.Core;

namespace DependencyStore.Domain.Configuration
{
  [XmlRoot("DependencyStore")]
  public class DependencyStoreConfiguration
  {
    private readonly List<ProjectConfiguration> _projectConfigurations = new List<ProjectConfiguration>();
    private FileAndDirectoryRules _fileAndDirectoryRules;
    private string _repositoryName;

    [XmlAttribute]
    public string RepositoryName
    {
      get { return _repositoryName; }
      set { _repositoryName = value; }
    }

    public List<ProjectConfiguration> ProjectConfigurations
    {
      get { return _projectConfigurations; }
    }

    [XmlIgnore]
    public Purl RepositoryDirectory
    {
      get
      {
        Purl purl = new Purl(ConfigurationPaths.RootDataDirectory);
        return purl.Join(this.RepositoryName);
      }
    }

    [XmlIgnore]
    public FileAndDirectoryRules FileAndDirectoryRules
    {
      get { return _fileAndDirectoryRules; }
      set { _fileAndDirectoryRules = value; }
    }

    public virtual void EnsureValid()
    {
      if (String.IsNullOrEmpty(_repositoryName))
      {
        throw new ConfigurationException("Invalid Repository Directory!");
      }
      foreach (ProjectConfiguration configuration in _projectConfigurations)
      {
        configuration.EnsureValid();
      }
    }
  }
}
