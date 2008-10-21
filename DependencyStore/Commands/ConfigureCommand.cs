using System;
using System.Collections.Generic;

using DependencyStore.Application;

namespace DependencyStore.Commands
{
  public class ConfigureCommand : Command
  {
    private readonly IProjectState _projectState;
    private string _repositoryName = "Default";

    public string RepositoryName
    {
      get { return _repositoryName; }
      set { _repositoryName = value; }
    }

    public ConfigureCommand(IProjectState projectState)
    {
      _projectState = projectState;
    }

    public override CommandStatus Run()
    {
      if (!_projectState.Configure(_repositoryName))
      {
        return CommandStatus.Failure;
      }
      Console.WriteLine("Saved configuration...");
      return CommandStatus.Success;
    }
  }
}
