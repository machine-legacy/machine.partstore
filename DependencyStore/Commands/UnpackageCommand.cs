using System;

using DependencyStore.Domain.Core;
using DependencyStore.Domain.Core.Repositories;

namespace DependencyStore.Commands
{
  public class UnpackageCommand : Command
  {
    private readonly ICurrentProjectRepository _currentProjectRepository;
    private readonly IRepositorySetRepository _repositorySetRepository;

    public UnpackageCommand(ICurrentProjectRepository currentProjectRepository, IRepositorySetRepository repositorySetRepository)
    {
      _currentProjectRepository = currentProjectRepository;
      _repositorySetRepository = repositorySetRepository;
    }

    public override CommandStatus Run()
    {
      new ArchiveProgressDisplayer(false);
      RepositorySet repositorySet = _repositorySetRepository.FindDefaultRepositorySet();
      CurrentProject project = _currentProjectRepository.FindCurrentProject();
      if (!project.AreAllReferencesHealthy)
      {
        Console.WriteLine("Not all project references are healthy!");
        Console.WriteLine("Use 'ds show' to see which ones you need to fix");
        Console.WriteLine("Use 'ds refresh' to download missing versions (maybe)");
        return CommandStatus.Failure;
      }
      project.UnpackageIfNecessary(repositorySet);
      return CommandStatus.Success;
    }
  }
}