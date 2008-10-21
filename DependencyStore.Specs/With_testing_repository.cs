using System;

using DependencyStore.Commands;

using Machine.Container;
using Machine.Specifications;

namespace DependencyStore
{
  public class with_container
  {
    protected static MachineContainer container;

    Establish context = () =>
    {
      container = new MachineContainer();
      container.Initialize();
      container.PrepareForServices();
      ContainerRegistrationHelper helper = new ContainerRegistrationHelper(container);
      helper.AddServiceCollectionsFrom(typeof(DependencyStoreServices).Assembly);
      container.Start();
      IoC.Container = container;
    };
  }

  public class with_testing_repository : with_container
  {
    protected static TestingRepository repository;

    Establish context = () =>
    {
      repository = new TestingRepository();
    };

    Cleanup after = () =>
    {
      repository.Cleanup();
    };
  }

  public class with_testing_repository_and_blank_directory : with_testing_repository
  {
    protected static TestingProject project;

    Establish context = () =>
    {
      project = new TestingProject();
      project.Create();
    };

    Cleanup after = () =>
    {
      project.Cleanup();
    };
  }

  public class with_testing_repository_and_blank_project : with_testing_repository_and_blank_directory
  {
    Establish context = () =>
    {
      project.Configure();
    };
  }
}
