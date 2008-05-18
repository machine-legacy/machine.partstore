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
      container.AddService<ConfigurationPaths>();

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
      ConfigurationPaths configuration = container.Resolve<ConfigurationPaths>();
      string path = configuration.FindConfigurationPath();
      if (dryRun)
      {
        controller.Show(configurationRepository.FindConfiguration(path));
      }
      else
      {
        controller.Update(configurationRepository.FindConfiguration(path));
      }
    }
  }
}