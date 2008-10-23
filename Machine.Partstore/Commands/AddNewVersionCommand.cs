using System;
using System.Collections.Generic;

using Machine.Partstore.Application;

namespace Machine.Partstore.Commands
{
  public class AddNewVersionCommand : Command
  {
    private readonly IManipulateRepositorySets _repositorySets;
    private string _repositoryName;
    private string _tags;

    public string RepositoryName
    {
      get { return _repositoryName; }
      set { _repositoryName = value; }
    }

    public string Tags
    {
      get { return _tags; }
      set { _tags = value; }
    }

    public AddNewVersionCommand(IManipulateRepositorySets repositorySets)
    {
      _repositorySets = repositorySets;
    }

    public override CommandStatus Run()
    {
      new ArchiveProgressDisplayer(true);
      AddingVersionResponse response = _repositorySets.AddNewVersion(_repositoryName, _tags);
      if (response.NoBuildDirectory)
      {
        Console.WriteLine("Current project has no Build directory configured.");
        return CommandStatus.Failure;
      }
      if (response.BuildDirectoryEmpty)
      {
        Console.WriteLine("Current project has nothing in its build directory.");
        return CommandStatus.Failure;
      }
      if (response.AmbiguousRepositoryName)
      {
        Console.WriteLine("Repository to add new version to is required when you have more than 1 repository.");
        return CommandStatus.Failure;
      }
      return CommandStatus.Success;
    }
  }
}