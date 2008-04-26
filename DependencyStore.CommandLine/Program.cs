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
        /*
        else
        {
          DependencyStoreConfiguration configuration = configurationRepository.FindConfiguration(arg);
        }
        */
      }
      IController controller = container.Resolve<IController>();
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
}