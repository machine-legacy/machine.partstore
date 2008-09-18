using System;

using Machine.Core.Utility;

using DependencyStore.Domain.Distribution;
using DependencyStore.Domain.Distribution.Repositories;

namespace DependencyStore.Commands
{
  public class ShowCommand : Command
  {
    private readonly ICurrentProjectRepository _currentProjectRepository;

    public ShowCommand(ICurrentProjectRepository currentProjectRepository)
    {
      _currentProjectRepository = currentProjectRepository;
    }

    public override CommandStatus Run()
    {
      CurrentProject project = _currentProjectRepository.FindCurrentProject();
      foreach (ProjectReference reference in project.References)
      {
        TimeSpan age = DateTime.Now - reference.DesiredVersion.CreatedAt;
        Console.WriteLine("{0} references {1} ({2} old)", reference.ParentProject, reference.Dependency, TimeSpanHelper.ToPrettyString(age));
      }
      return CommandStatus.Success;
    }
  }
}