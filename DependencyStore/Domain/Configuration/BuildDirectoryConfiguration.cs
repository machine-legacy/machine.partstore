using System;
using System.Xml.Serialization;

using DependencyStore.Domain.FileSystem;

namespace DependencyStore.Domain.Configuration
{
  [XmlType("Build")]
  public class BuildDirectoryConfiguration : DirectoryConfiguration
  {
    public BuildDirectoryConfiguration()
    {
    }

    public BuildDirectoryConfiguration(string path)
      : base(path)
    {
    }

    public BuildDirectoryConfiguration(Purl path)
      : base(path)
    {
    }
  }
}