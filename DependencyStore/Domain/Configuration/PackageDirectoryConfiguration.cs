using System.Xml.Serialization;

namespace DependencyStore.Domain.Configuration
{
  [XmlType("Package")]
  public class PackageDirectoryConfiguration
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

    public PackageDirectoryConfiguration()
    {
    }

    public PackageDirectoryConfiguration(string path)
    {
      _path = path;
    }
  }
}