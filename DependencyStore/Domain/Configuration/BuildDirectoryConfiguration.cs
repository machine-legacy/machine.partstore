using System;
using System.Xml.Serialization;

namespace DependencyStore.Domain.Configuration
{
  [XmlType("Build")]
  public class BuildDirectoryConfiguration
  {
    private string _path;

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
      if (String.IsNullOrEmpty(_path))
      {
        throw new ConfigurationException("Missing Path!");
      }
    }
  }
}