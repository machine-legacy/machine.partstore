using System;
using System.Collections.Generic;

namespace DependencyStore.Commands
{
  public class ConfigureCommand : Command
  {
    public override CommandStatus Run()
    {
      return CommandStatus.Success;
    }
  }
}
