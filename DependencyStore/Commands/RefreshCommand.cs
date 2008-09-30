using System;

using DependencyStore.Domain.Core;
using DependencyStore.Domain.Core.Repositories;

namespace DependencyStore.Commands
{
  public class RefreshCommand : Command
  {
    private readonly IRepositorySetRepository _repositorySetRepository;
    private readonly IRepositoryRepository _repositoryRepository;

    public RefreshCommand(IRepositorySetRepository repositorySetRepository, IRepositoryRepository repositoryRepository)
    {
      _repositorySetRepository = repositorySetRepository;
      _repositoryRepository = repositoryRepository;
    }

    public override CommandStatus Run()
    {
      RepositorySet repositorySet = _repositorySetRepository.FindDefaultRepositorySet();
      foreach (Repository repository in repositorySet.Repositories)
      {
        _repositoryRepository.RefreshRepository(repository);
      }
      return CommandStatus.Success;
    }
  }
}