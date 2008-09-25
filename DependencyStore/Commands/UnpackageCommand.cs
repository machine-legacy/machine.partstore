using System;

using DependencyStore.Domain.Distribution;
using DependencyStore.Domain.Distribution.Repositories;

namespace DependencyStore.Commands
{
  public class UnpackageCommand : Command
  {
    private readonly ICurrentProjectRepository _currentProjectRepository;
    private readonly IRepositoryRepository _repositoryRepository;

    public UnpackageCommand(ICurrentProjectRepository currentProjectRepository, IRepositoryRepository repositoryRepository)
    {
      _currentProjectRepository = currentProjectRepository;
      _repositoryRepository = repositoryRepository;
    }

    public override CommandStatus Run()
    {
      new ArchiveProgressDisplayer(false);
      Repository repository = _repositoryRepository.FindDefaultRepository();
      CurrentProject project = _currentProjectRepository.FindCurrentProject();
      if (!project.AreAllReferencesHealthy)
      {
        Console.WriteLine("Not all project references are healthy!");
        Console.WriteLine("Use 'ds show' to see which ones you need to fix...");
        return CommandStatus.Failure;
      }
      project.UnpackageIfNecessary(repository);
      return CommandStatus.Success;
    }
  }
}