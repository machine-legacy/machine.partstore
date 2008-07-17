using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using DependencyStore.Domain;
using DependencyStore.Domain.Configuration;

using Machine.Core.Services;
using Machine.Core.Utility;

namespace DependencyStore.Services.DataAccess.Impl
{
  public class ConfigurationRepository : IConfigurationRepository
  {
    private readonly IFileSystem _fileSystem;

    public ConfigurationRepository(IFileSystem fileSystem)
    {
      _fileSystem = fileSystem;
    }

    #region IConfigurationRepository Members
    public DependencyStoreConfiguration FindConfiguration(string configurationFile)
    {
      try
      {
        using (StreamReader reader = _fileSystem.OpenText(configurationFile))
        {
          DependencyStoreConfiguration configuration = XmlSerializationHelper.DeserializeString<DependencyStoreConfiguration>(reader.ReadToEnd());
          configuration.PackageDirectory = new FileSystemPath(@"D:\Home\Source\Packages");
          return configuration;
        }
      }
      catch (InvalidOperationException e)
      {
        throw new InvalidConfigurationException("Error reading configuration", e);
      }
      catch (XmlException e)
      {
        throw new InvalidConfigurationException("Error reading configuration", e);
      }
    }
    #endregion
  }
}
