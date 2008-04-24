using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

using DependencyStore.Domain.Configuration;

using Machine.Core.Services;
using Machine.Core.Utility;

namespace DependencyStore.Services.DataAccess
{
  public class ConfigurationRepository : IConfigurationRepository
  {
    private readonly IFileSystem _fileSystem;
    private string _configurationFile = @"DependencyStore.config";

    public string ConfigurationFile
    {
      get { return _configurationFile; }
      set { _configurationFile = value; }
    }

    public ConfigurationRepository(IFileSystem fileSystem)
    {
      _fileSystem = fileSystem;
    }

    #region IConfigurationRepository Members
    public DependencyStoreConfiguration FindConfiguration()
    {
      try
      {
        using (StreamReader reader = _fileSystem.OpenText(this.ConfigurationFile))
        {
          return XmlSerializationHelper.DeserializeString<DependencyStoreConfiguration>(reader.ReadToEnd());
        }
      }
      catch (XmlException e)
      {
        throw new InvalidConfigurationException("Error reading configuration", e);
      }
    }
    #endregion
  }
}
