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
      foreach (string arg in args)
      {
        IConfigurationRepository configurationRepository = container.Resolve<IConfigurationRepository>();
        DependencyStoreConfiguration configuration = configurationRepository.FindConfiguration(arg);
      }
      container.Resolve<IController>().Show();
    }
  }
}