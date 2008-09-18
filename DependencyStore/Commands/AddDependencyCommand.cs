using System;

using DependencyStore.Domain.Distribution;
using DependencyStore.Domain.Distribution.Repositories;

namespace DependencyStore.Commands
{
  public class AddDependencyCommand : Command
  {
    private readonly ICurrentProjectRepository _currentProjectRepository;
    private readonly IRepositoryRepository _repositoryRepository;

    public AddDependencyCommand(ICurrentProjectRepository currentProjectRepository, IRepositoryRepository repositoryRepository)
    {
      _repositoryRepository = repositoryRepository;
      _currentProjectRepository = currentProjectRepository;
    }

    private string _projectToAdd;

    public string ProjectToAdd
    {
      get { return _projectToAdd; }
      set { _projectToAdd = value; }
    }

    public override CommandStatus Run()
    {
      Repository repository = _repositoryRepository.FindDefaultRepository();
      ArchivedProject dependency = repository.FindProject(this.ProjectToAdd);
      if (dependency == null)
      {
        Console.WriteLine("Project not found: {0}", this.ProjectToAdd);
        return CommandStatus.Failure;
      }
      Console.WriteLine("Adding reference to {0} ({1})", dependency.Name, dependency.LatestVersion.CreatedAt);
      CurrentProject project = _currentProjectRepository.FindCurrentProject();
      project.AddReferenceToLatestVersion(dependency);
      _currentProjectRepository.SaveCurrentProject(project);
      return CommandStatus.Success;
    }
  }
}