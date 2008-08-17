using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace DependencyStore.Domain.Configuration
{
  [XmlType("Project")]
  public class ProjectConfiguration
  {
    private string _name;
    private BuildDirectoryConfiguration _rootConfiguration;
    private BuildDirectoryConfiguration _buildConfiguration;
    private LibraryDirectoryConfiguration _libraryConfiguration;

    [XmlAttribute]
    public string Name
    {
      get { return _name; }
      set { _name = value; }
    }

    public BuildDirectoryConfiguration Root
    {
      get { return _rootConfiguration; }
      set { _rootConfiguration = value; }
    }

    public BuildDirectoryConfiguration Build
    {
      get { return _buildConfiguration; }
      set { _buildConfiguration = value; }
    }

    public LibraryDirectoryConfiguration Library
    {
      get { return _libraryConfiguration; }
      set { _libraryConfiguration = value; }
    }

    public void EnsureValid()
    {
      if (String.IsNullOrEmpty(_name))
      {
        throw new ConfigurationException("Missing Name!");
      }
    }
  }
}
