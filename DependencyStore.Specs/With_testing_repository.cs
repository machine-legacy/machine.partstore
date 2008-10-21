using System;
using System.Collections.Generic;
using System.IO;
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

  public class with_testing_repository_and_blank_project : with_testing_repository
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

  public class TestingRepository
  {
    private readonly string _directory;

    public string RootDirectory
    {
      get { return _directory; }
    }

    public TestingRepository()
    {
      _directory = Path.Combine(Path.GetTempPath(), "TestRepository");
      Directory.CreateDirectory(_directory);
    }

    public void Cleanup()
    {
      Directory.Delete(_directory);
    }
  }

  public class TestingProject
  {
    private readonly string _directory;

    public string RootDirectory
    {
      get { return _directory; }
    }

    public bool HasConfiguration
    {
      get { return File.Exists(PathTo("DependencyStore.config")); }
    }

    public TestingProject()
    {
      _directory = Path.Combine(Path.GetTempPath(), "TestProject");
    }

    private string PathTo(string path)
    {
      return Path.Combine(_directory, path);
    }

    public void Create()
    {
      Environment.CurrentDirectory = Environment.GetFolderPath(Environment.SpecialFolder.System);
      if (Directory.Exists(_directory))
      {
        Directory.Delete(_directory, true);
      }
      Directory.CreateDirectory(_directory);
      Environment.CurrentDirectory = _directory;
      AddRootClue();
    }

    public void RemoveRootClue()
    {
      File.Delete(PathTo(".gitignore"));
    }

    public void AddRootClue()
    {
      File.WriteAllText(PathTo(".gitignore"), String.Empty);
    }

    public void Cleanup()
    {
      Directory.Delete(_directory);
    }
  }
}
