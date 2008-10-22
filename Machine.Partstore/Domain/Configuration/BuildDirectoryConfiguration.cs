using System;
using System.Xml.Serialization;

using Machine.Partstore.Domain.FileSystem;

namespace Machine.Partstore.Domain.Configuration
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