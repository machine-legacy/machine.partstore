using System;
using System.Collections.Generic;

using Machine.Core.Utility;

using DependencyStore.Domain.Distribution;
using DependencyStore.Domain.Distribution.Repositories;

namespace DependencyStore.Commands
{
  public abstract class Command : ICommand
  {
    public abstract void Run();
  }
  public class ShowCommand : Command
  {
    private readonly ICurrentProjectRepository _currentProjectRepository;

    public ShowCommand(ICurrentProjectRepository currentProjectRepository)
    {
      _currentProjectRepository = currentProjectRepository;
    }

    public override void Run()
    {
      CurrentProject project = _currentProjectRepository.FindCurrentProject();
      foreach (ProjectReference reference in project.References)
      {
        TimeSpan age = DateTime.Now - reference.DesiredVersion.CreatedAt;
        Console.WriteLine("{0} references {1} ({2} old)", reference.ParentProject, reference.Dependency, TimeSpanHelper.ToPrettyString(age));
      }
    }
  }
  public class SeachAvailableProjects : Command
  {
    private readonly IRepositoryRepository _repositoryRepository;

    public SeachAvailableProjects(IRepositoryRepository repositoryRepository)
    {
      _repositoryRepository = repositoryRepository;
    }

    public override void Run()
    {
      Repository repository = _repositoryRepository.FindDefaultRepository();
      foreach (ArchivedProject project in repository.Projects)
      {
        Console.WriteLine("{0,-30} {1}", project.Name, project.LatestVersion.CreatedAt);
      }
    }
  }
  public class UnpackageCommand : Command
  {
    private readonly ICurrentProjectRepository _currentProjectRepository;

    public UnpackageCommand(ICurrentProjectRepository currentProjectRepository)
    {
      _currentProjectRepository = currentProjectRepository;
    }

    public override void Run()
    {
      CurrentProject project = _currentProjectRepository.FindCurrentProject();
      project.InstallPackagesIfNecessary();
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
  public class PublishNewVersionCommand : Command
  {
    private readonly ICurrentProjectRepository _currentProjectRepository;
    private readonly IRepositoryRepository _repositoryRepository;

    public PublishNewVersionCommand(ICurrentProjectRepository currentProjectRepository, IRepositoryRepository repositoryRepository)
    {
      _currentProjectRepository = currentProjectRepository;
      _repositoryRepository = repositoryRepository;
    }

    public override void Run()
    {
      Repository repository = _repositoryRepository.FindDefaultRepository();
      CurrentProject project = _currentProjectRepository.FindCurrentProject();
      project.PublishNewVersion(repository);
      _repositoryRepository.SaveRepository(repository);
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
      Console.WriteLine("  search");
      Console.WriteLine("  help");
    }
  }
}
