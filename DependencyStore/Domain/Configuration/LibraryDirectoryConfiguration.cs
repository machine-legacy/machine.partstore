using System;
using System.Xml.Serialization;

namespace DependencyStore.Domain.Configuration
{
  [XmlType("Library")]
  public class LibraryDirectoryConfiguration
  {
    private string _path;

    [XmlAttribute]
    public string Path
    {
      get { return _path; }
      set { _path = value; }
    }

    [XmlIgnore]
    public Purl AsPurl
    {
      get { return new Purl(_path); }
    }

    public LibraryDirectoryConfiguration()
    {
    }

    public LibraryDirectoryConfiguration(string path)
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