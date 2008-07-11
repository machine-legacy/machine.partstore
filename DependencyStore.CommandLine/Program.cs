using DependencyStore.Services;
using DependencyStore.Domain.Configuration;
using DependencyStore.Services.DataAccess;

namespace DependencyStore.CommandLine
{
  public class Program
  {
    public static void Main(string[] args)
    {
      using (DependencyStoreContainer container = new DependencyStoreContainer())
      {
        container.Initialize();
        container.PrepareForServices();
        container.Start();
        container.Add<ConfigurationPaths>();

        IConfigurationRepository configurationRepository = container.Resolve.Object<IConfigurationRepository>();
        bool dryRun = false;
        foreach (string arg in args)
        {
          if (arg == "--dry")
          {
            dryRun = true;
          }
        }
        IController controller = container.Resolve.Object<IController>();
        ConfigurationPaths configuration = container.Resolve.Object<ConfigurationPaths>();
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
      System.Console.ReadKey();
    }
  }
}