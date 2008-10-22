using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Machine.Partstore.Domain.FileSystem;

namespace Machine.Partstore.Domain.Configuration
{
  public class IncludeRepository
  {
    private string _name;

    [XmlAttribute]
    public string Name
    {
      get { return _name; }
      set { _name = value; }
    }

    [XmlIgnore]
    public Purl RepositoryDirectory
    {
      get { return BuildDirectory(); }
    }

    private IncludeRepository()
    {
    }

    public IncludeRepository(string name)
    {
      _name = name;
    }

    public virtual void EnsureValid()
    {
      if (String.IsNullOrEmpty(_name))
      {
        throw new ConfigurationException("Invalid Repository Directory!");
      }
    }

    private Purl BuildDirectory()
    {
      Purl purl = new Purl(ConfigurationPaths.RootDataDirectory);
      return purl.Join(this.Name);
    }
  }
}
