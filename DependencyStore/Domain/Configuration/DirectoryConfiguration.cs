using System;
using System.Xml.Serialization;

using DependencyStore.Domain.Core;

namespace DependencyStore.Domain.Configuration
{
  public abstract class DirectoryConfiguration
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

    protected DirectoryConfiguration()
    {
    }

    protected DirectoryConfiguration(string path)
    {
      _path = path;
    }

    protected DirectoryConfiguration(Purl path)
    {
      _path = path.AsString;
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