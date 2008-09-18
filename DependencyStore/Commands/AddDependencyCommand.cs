using System;

using DependencyStore.Domain.Distribution;
using DependencyStore.Domain.Distribution.Repositories;

namespace DependencyStore.Commands
{
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
}