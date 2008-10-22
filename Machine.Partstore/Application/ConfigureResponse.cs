using System;
using System.Collections.Generic;

namespace DependencyStore.Application
{
  public class ConfigureResponse
  {
    private readonly string _configurationFile;

    public string ConfigurationFile
    {
      get { return _configurationFile; }
    }

    public bool Success
    {
      get { return true; }
    }

    public ConfigureResponse(string configurationFile)
    {
      _configurationFile = configurationFile;
    }
  }
}
