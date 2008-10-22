using System;
using System.Collections.Generic;

using Machine.Partstore.Application;

namespace Machine.Partstore.Commands
{
  public class AddDependencyCommand : Command
  {
    private readonly IManipulateProjectDependencies _manipulateProjectDependencies;
    private string _repositoryName;
    private string _projectName;

    public string RepositoryName
    {
      get { return _repositoryName; }
      set { _repositoryName = value; }
    }

    public string ProjectName
    {
      get { return _projectName; }
      set { _projectName = value; }
    }

    public AddDependencyCommand(IManipulateProjectDependencies manipulateProjectDependencies)
    {
      _manipulateProjectDependencies = manipulateProjectDependencies;
    }

    public override CommandStatus Run()
    {
      new ArchiveProgressDisplayer(false);
      AddingDependencyResponse response = _manipulateProjectDependencies.AddDependency(_repositoryName, _projectName);
      if (response.NoMatchingProject)
      {
        Console.WriteLine("Project not found: {0}", this.ProjectName);
        return CommandStatus.Failure;
      }
      if (response.AmbiguousProjectName)
      {
        Console.WriteLine("Too many projects found matching that criteria:");
        return CommandStatus.Failure;
      }
      Console.WriteLine("Adding reference to {0}", _projectName);
      return CommandStatus.Success;
    }
  }
}