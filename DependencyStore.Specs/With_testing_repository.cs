using System;
using System.Collections.Generic;

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
    static TestingRepository repository;

    Establish context = () =>
    {
      repository = new TestingRepository();
    };
  }

  public class with_testing_repository_and_blank_project : with_testing_repository
  {
    static TestingProject project;

    Establish context = () =>
    {
      project = new TestingProject();
    };
  }

  public class TestingRepository
  {
    private readonly string _directory;

    public string Directory
    {
      get { return _directory; }
    }

    public TestingRepository()
    {
    }
  }

  public class TestingProject
  {
    private readonly string _directory;

    public string Directory
    {
      get { return _directory; }
    }

    public TestingProject()
    {
    }
  }
}
