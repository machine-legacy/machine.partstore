using System;

using DependencyStore.Domain.Core;
using DependencyStore.Domain.Core.Repositories;

namespace DependencyStore.Commands
{
  public class RefreshCommand : Command
  {
    private readonly IRepositoryRepository _repositoryRepository;

    public RefreshCommand(IRepositoryRepository repositoryRepository)
    {
      _repositoryRepository = repositoryRepository;
    }

    public override CommandStatus Run()
    {
      Repository repository = _repositoryRepository.FindDefaultRepository();
      _repositoryRepository.RefreshRepository(repository);
      return CommandStatus.Success;
    }
  }
}