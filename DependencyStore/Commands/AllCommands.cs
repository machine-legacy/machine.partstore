using System;
using System.Collections.Generic;

using Machine.Core.Utility;

using DependencyStore.Domain.Repositories;
using DependencyStore.Domain.Repositories.Repositories;

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
    private readonly ICurrentProjectRepository _projectRepository;
    private readonly IRepositoryRepository _repositoryRepository;

    public AddDependencyCommand(ICurrentProjectRepository projectRepository, IRepositoryRepository repositoryRepository)
    {
      _projectRepository = projectRepository;
      _repositoryRepository = repositoryRepository;
    }

    private string _projectToAdd;

    public string ProjectToAdd
    {
      get { return _projectToAdd; }
      set { _projectToAdd = value; }
    }

    public override void Run()
    {
      Repository repository = _repositoryRepository.FindDefaultRepository();
      ArchivedProject projectBeingReferenced = repository.FindProject(this.ProjectToAdd);
      if (projectBeingReferenced == null)
      {
        Console.WriteLine("Project not found: {0}", this.ProjectToAdd);
        return;
      }
      CurrentProject project = _projectRepository.FindCurrentProject();
      project.AddReferenceToLatestVersion(projectBeingReferenced);
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
