using System;

using DependencyStore.Domain.Core;
using DependencyStore.Domain.Core.Repositories;

namespace DependencyStore.Commands
{
  public class RefreshCommand : Command
  {
    private readonly IRepositorySetRepository _repositorySetRepository;

    public RefreshCommand(IRepositorySetRepository repositorySetRepository)
    {
      _repositorySetRepository = repositorySetRepository;
    }

    public override CommandStatus Run()
    {
      RepositorySet repositorySet = _repositorySetRepository.FindDefaultRepositorySet();
      repositorySet.Refresh();
      return CommandStatus.Success;
    }
  }
}