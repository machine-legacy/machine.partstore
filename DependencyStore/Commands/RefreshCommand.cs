using System;

using DependencyStore.Application;

namespace DependencyStore.Commands
{
  public class RefreshCommand : Command
  {
    private readonly IManipulateRepositorySets _repositorySets;

    public RefreshCommand(IManipulateRepositorySets repositorySets)
    {
      _repositorySets = repositorySets;
    }

    public override CommandStatus Run()
    {
      if (!_repositorySets.Refresh())
      {
        return CommandStatus.Failure;
      }
      return CommandStatus.Success;
    }
  }
}