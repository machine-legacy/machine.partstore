using System;
using System.Collections.Generic;

using DependencyStore.Application;

namespace DependencyStore.Commands
{
  public class ConfigureCommand : Command
  {
    private readonly IAmForProjectState _projectState;
    private string _repositoryName = "Default";

    public string RepositoryName
    {
      get { return _repositoryName; }
      set { _repositoryName = value; }
    }

    public ConfigureCommand(IAmForProjectState projectState)
    {
      _projectState = projectState;
    }

    public override CommandStatus Run()
    {
      ConfigureResponse response = _projectState.Configure(_repositoryName);
      if (!response.Success)
      {
        return CommandStatus.Failure;
      }
      Console.WriteLine("Saved configuration: " + response.ConfigurationFile);
      return CommandStatus.Success;
    }
  }
}
