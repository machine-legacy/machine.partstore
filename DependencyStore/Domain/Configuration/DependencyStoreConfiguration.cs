using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace DependencyStore.Domain.Configuration
{
  [XmlRoot("DependencyStore")]
  public class DependencyStoreConfiguration
  {
    private readonly List<BuildDirectoryConfiguration> _buildDirectories = new List<BuildDirectoryConfiguration>();
    private readonly List<LibraryDirectoryConfiguration> _libraryDirectories = new List<LibraryDirectoryConfiguration>();

    public List<BuildDirectoryConfiguration> BuildDirectories
    {
      get { return _buildDirectories; }
    }

    public List<LibraryDirectoryConfiguration> LibraryDirectories
    {
      get { return _libraryDirectories; }
    }
  }
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
  }
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
