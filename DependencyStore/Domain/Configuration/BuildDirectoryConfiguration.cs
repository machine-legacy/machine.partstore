using System;
using System.Xml.Serialization;

namespace DependencyStore.Domain.Configuration
{
  [XmlType("Build")]
  public class BuildDirectoryConfiguration
  {
    private string _name;
    private string _path;

    [XmlAttribute]
    public string Name
    {
      get { return _name; }
      set { _name = value; }
    }

    [XmlAttribute]
    public string Path
    {
      get { return _path; }
      set { _path = value; }
    }

    [XmlIgnore]
    public FileSystemPath AsFileSystemPath
    {
      get { return new FileSystemPath(_path); }
    }

    public BuildDirectoryConfiguration()
    {
    }

    public BuildDirectoryConfiguration(string path)
    {
      _path = path;
    }

    public void EnsureValid()
    {
      if (String.IsNullOrEmpty(_name))
      {
        throw new ConfigurationException("Missing Name!");
      }
      if (String.IsNullOrEmpty(_path))
      {
        throw new ConfigurationException("Missing Path!");
      }
    }
  }
}