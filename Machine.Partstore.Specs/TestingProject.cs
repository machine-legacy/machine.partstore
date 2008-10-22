using System;
using System.IO;

using Machine.Partstore.Commands;

using Machine.Container;

namespace Machine.Partstore
{
  public class TestingProject : DirectoryManipulator
  {
    public bool HasConfiguration
    {
      get { return File.Exists(PathTo("DependencyStore.config")); }
    }

    public TestingProject()
      : base(Path.Combine(Path.GetTempPath(), "TestProject"))
    {
    }

    public override void Create()
    {
      base.Create();
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

    public void AddBuild()
    {
      Directory.CreateDirectory(PathTo("Build"));
      File.WriteAllText(PathTo(@"Build\Library.dll"), "Library");
    }

    public void AddLibraries()
    {
      Directory.CreateDirectory(PathTo("Libraries"));
    }

    public void Configure()
    {
      ConfigureCommand configure = IoC.Container.Resolve.Object<ConfigureCommand>();
      configure.RepositoryName = "TestRepository";
      configure.Run();
    }
  }
}