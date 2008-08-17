using System;
using System.Xml.Serialization;

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
  }
}