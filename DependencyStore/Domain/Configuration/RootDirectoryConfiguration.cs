using System;
using System.Xml.Serialization;

namespace DependencyStore.Domain.Configuration
{
  [XmlType("Root")]
  public class RootDirectoryConfiguration : DirectoryConfiguration
  {
    public RootDirectoryConfiguration()
    {
    }

    public RootDirectoryConfiguration(string path)
      : base(path)
    {
    }

    public RootDirectoryConfiguration(Purl path)
      : base(path)
    {
    }
  }
}