using System;

using DependencyStore.Application;

namespace DependencyStore.Commands
{
  public class RefreshCommand : Command
  {
    private readonly IManipulateRepositories _repositories;

    public RefreshCommand(IManipulateRepositories repositories)
    {
      _repositories = repositories;
    }

    public override CommandStatus Run()
    {
      if (!_repositories.Refresh())
      {
        return CommandStatus.Failure;
      }
      return CommandStatus.Success;
    }
  }
}