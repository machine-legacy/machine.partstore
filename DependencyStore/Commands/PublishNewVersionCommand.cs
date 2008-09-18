using System;

using DependencyStore.Domain.Distribution;
using DependencyStore.Domain.Distribution.Repositories;

namespace DependencyStore.Commands
{
  public class PublishNewVersionCommand : Command
  {
    private readonly ICurrentProjectRepository _currentProjectRepository;
    private readonly IRepositoryRepository _repositoryRepository;

    public PublishNewVersionCommand(ICurrentProjectRepository currentProjectRepository, IRepositoryRepository repositoryRepository)
    {
      _currentProjectRepository = currentProjectRepository;
      _repositoryRepository = repositoryRepository;
    }

    public override CommandStatus Run()
    {
      Repository repository = _repositoryRepository.FindDefaultRepository();
      CurrentProject project = _currentProjectRepository.FindCurrentProject();
      project.PublishNewVersion(repository);
      _repositoryRepository.SaveRepository(repository);
      return CommandStatus.Success;
    }
  }
}