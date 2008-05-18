using System;
using System.Collections.Generic;

using DependencyStore.Services;
using DependencyStore.Domain.Configuration;
using DependencyStore.Services.DataAccess;

namespace DependencyStore.CommandLine
{
  public class Program
  {
    public static void Main(string[] args)
    {
      DependencyStoreContainer container = new DependencyStoreContainer();
      container.Initialize();

      IConfigurationRepository configurationRepository = container.Resolve<IConfigurationRepository>();
      bool dryRun = false;
      foreach (string arg in args)
      {
        if (arg == "--dry")
        {
          dryRun = true;
        }
      }
      IController controller = container.Resolve<IController>();
      ConfigurationPaths configurationPaths = new ConfigurationPaths();
      if (dryRun)
      {
        controller.Show(configurationRepository.FindConfiguration("DependencyStore.config"));
      }
      else
      {
        controller.Update(configurationRepository.FindConfiguration("DependencyStore.config"));
      }
    }
  }
  public class ConfigurationPaths
  {
    public string[] FindAllPossiblePaths()
    {
      List<string> paths = new List<string>();
      paths.Add(@"DependencyStore.config");
      paths.Add(@"C:\DependencyStore.config");
      paths.Add(@"C:\DependencyStore.config");
      return paths.ToArray();
    }
  }
}