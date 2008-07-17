using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace DependencyStore.Domain.Configuration
{
  [XmlType("Project")]
  public class ProjectConfiguration
  {
    private string _name;
    private BuildDirectoryConfiguration _buildConfiguration;

    [XmlAttribute]
    public string Name
    {
      get { return _name; }
      set { _name = value; }
    }

    public BuildDirectoryConfiguration Build
    {
      get { return _buildConfiguration; }
      set { _buildConfiguration = value; }
    }

    public void EnsureValid()
    {
      if (String.IsNullOrEmpty(_name))
      {
        throw new ConfigurationException("Missing Name!");
      }
      if (_buildConfiguration == null)
      {
        throw new ConfigurationException("Missing Build Configuration!");
      }
      _buildConfiguration.EnsureValid();
    }
  }
}
