using Machine.Container;

using DependencyStore.Domain;
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
        IoC.Container = container;

        IConfigurationRepository configurationRepository = container.Resolve.Object<IConfigurationRepository>();
        bool dryRun = false;
        bool archiving = false;
        foreach (string arg in args)
        {
          if (arg == "--archive")
          {
            archiving = true;
          }
          if (arg == "--dry")
          {
            dryRun = true;
          }
        }
        IController controller = container.Resolve.Object<IController>();
        DependencyStoreConfiguration configuration = configurationRepository.FindDefaultConfiguration();
        if (archiving)
        {
          IRepositoryRepository repositoryRepository = container.Resolve.Object<IRepositoryRepository>();
          Repository repository = repositoryRepository.FindDefaultRepository(configuration);
          controller.ArchiveProjects(configuration, repository);
          repositoryRepository.SaveRepository(repository, configuration);
        }
        else
        {
          if (dryRun)
          {
            controller.Show(configuration);
          }
          else
          {
            controller.Update(configuration);
          }
        }
      }
    }
  }
}