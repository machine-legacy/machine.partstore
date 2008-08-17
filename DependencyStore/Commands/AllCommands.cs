using System;
using System.Collections.Generic;

using DependencyStore.Domain.Repositories;
using DependencyStore.Services.DataAccess;
using Machine.Core.Utility;

namespace DependencyStore.Commands
{
  public abstract class Command : ICommand
  {
    public abstract void Run();
  }
  public class ShowCommand : Command
  {
    private readonly IProjectReferenceRepository _projectReferenceRepository;

    public ShowCommand(IProjectReferenceRepository projectReferenceRepository)
    {
      _projectReferenceRepository = projectReferenceRepository;
    }

    public override void Run()
    {
      foreach (ProjectReference reference in _projectReferenceRepository.FindAllProjectReferences())
      {
        TimeSpan age = DateTime.Now - reference.DesiredVersion.CreatedAt;
        Console.WriteLine("{0} references {1} ({2} old)", reference.ParentProject, reference.Dependency, TimeSpanHelper.ToPrettyString(age));
      }
    }
  }
  public class UnpackageCommand : Command
  {
    private readonly IProjectReferenceRepository _projectReferenceRepository;

    public UnpackageCommand(IProjectReferenceRepository projectReferenceRepository)
    {
      _projectReferenceRepository = projectReferenceRepository;
    }

    public override void Run()
    {
      foreach (ProjectReference reference in _projectReferenceRepository.FindAllProjectReferences())
      {
        reference.InstallPackageIfNecessary();
      }
    }
  }
  public class AddDependencyCommand : Command
  {
    private string _projectToAdd;

    public string ProjectToAdd
    {
      get { return _projectToAdd; }
      set { _projectToAdd = value; }
    }

    public override void Run()
    {
    }
  }
  public class AddNewVersionCommand : Command
  {
    public override void Run()
    {
    }
  }
  public class HelpCommand : Command
  {
    public override void Run()
    {
      Console.WriteLine("{0} <command> [options]", "DependencyStore.exe");
      Console.WriteLine("Commands:");
      Console.WriteLine("  show");
      Console.WriteLine("  unpackage --dry-run");
      Console.WriteLine("  add --dry-run");
      Console.WriteLine("  update --all --dry-run");
      Console.WriteLine("  publish --dry-run");
      Console.WriteLine("  help");
    }
  }
}
