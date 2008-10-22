using System;

using Machine.Partstore.Commands;

using Machine.Container;
using Machine.Specifications;

namespace Machine.Partstore
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

  public class with_testing_repository_and_blank_directory : with_container
  {
    protected static TestingRepository repository;
    protected static TestingProject project;

    Establish context = () =>
    {
      repository = new TestingRepository();
      project = new TestingProject();
      project.Create();
    };

    Cleanup after = () =>
    {
      project.Cleanup();
      repository.Cleanup();
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
