using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace DependencyStore.Domain.Configuration
{
  [XmlRoot("DependencyStore")]
  public class DependencyStoreConfiguration
  {
    private readonly List<ProjectConfiguration> _projectConfigurations = new List<ProjectConfiguration>();
    private readonly List<IncludeRepository> _repositories = new List<IncludeRepository>();
    private FileAndDirectoryRules _fileAndDirectoryRules;

    public List<ProjectConfiguration> ProjectConfigurations
    {
      get { return _projectConfigurations; }
    }

    public List<IncludeRepository> Repositories
    {
      get { return _repositories; }
    }

    public IncludeRepository DefaultRepository
    {
      get
      {
        foreach (IncludeRepository repository in _repositories)
        {
          return repository;
        }
        throw new InvalidOperationException("No repositories!");
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
      foreach (IncludeRepository repository in _repositories)
      {
        repository.EnsureValid();
      }
      foreach (ProjectConfiguration configuration in _projectConfigurations)
      {
        configuration.EnsureValid();
      }
    }
  }
}
