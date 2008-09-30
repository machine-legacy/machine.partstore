using System;

using DependencyStore.Domain.Core;
using DependencyStore.Domain.Core.Repositories;

namespace DependencyStore.Commands
{
  public class AddDependencyCommand : Command
  {
    private readonly ICurrentProjectRepository _currentProjectRepository;
    private readonly IRepositorySetRepository _repositorySetRepository;
    private string _projectToAdd;

    public string ProjectToAdd
    {
      get { return _projectToAdd; }
      set { _projectToAdd = value; }
    }

    public AddDependencyCommand(ICurrentProjectRepository currentProjectRepository, IRepositorySetRepository repositorySetRepository)
    {
      _repositorySetRepository = repositorySetRepository;
      _currentProjectRepository = currentProjectRepository;
    }

    public override CommandStatus Run()
    {
      new ArchiveProgressDisplayer(false);
      RepositorySet repositorySet = _repositorySetRepository.FindDefaultRepositorySet();
      ArchivedProject dependency = repositorySet.FindProject(this.ProjectToAdd);
      if (dependency == null)
      {
        Console.WriteLine("Project not found: {0}", this.ProjectToAdd);
        return CommandStatus.Failure;
      }
      Console.WriteLine("Adding reference to {0} ({1})", dependency.Name, dependency.LatestVersion.Number);
      CurrentProject project = _currentProjectRepository.FindCurrentProject();
      project.AddReferenceToLatestVersion(dependency);
      _currentProjectRepository.SaveCurrentProject(project, repositorySet);
      return CommandStatus.Success;
    }
  }
}