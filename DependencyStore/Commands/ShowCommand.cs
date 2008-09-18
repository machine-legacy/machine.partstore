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
      Console.WriteLine("Current Project: {0}", project.Name);
      Console.WriteLine("References:");
      foreach (ProjectReference reference in project.References)
      {
        TimeSpan age = DateTime.Now - reference.DesiredVersion.CreatedAt;
        Console.WriteLine("  {0} ({1} old)", reference.Dependency.Name, TimeSpanHelper.ToPrettyString(age));
        Console.WriteLine("  {0}", reference.DesiredVersion.ArchivePath.AsString);
      }
      return CommandStatus.Success;
    }
  }
}