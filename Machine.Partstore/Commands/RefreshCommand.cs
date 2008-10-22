using System;

using Machine.Partstore.Application;

namespace Machine.Partstore.Commands
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