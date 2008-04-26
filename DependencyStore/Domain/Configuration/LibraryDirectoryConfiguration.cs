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
    public FileSystemPath AsFileSystemPath
    {
      get { return new FileSystemPath(_path); }
    }

    public LibraryDirectoryConfiguration()
    {
    }

    public LibraryDirectoryConfiguration(string path)
    {
      _path = path;
    }
  }
}