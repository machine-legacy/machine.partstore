using System;

using DependencyStore.Domain.Distribution;
using DependencyStore.Domain.Distribution.Repositories;

namespace DependencyStore.Commands
{
  public class UnpackageCommand : Command
  {
    private readonly ICurrentProjectRepository _currentProjectRepository;

    public UnpackageCommand(ICurrentProjectRepository currentProjectRepository)
    {
      _currentProjectRepository = currentProjectRepository;
    }

    public override CommandStatus Run()
    {
      new ArchiveProgressDisplayer(false);
      CurrentProject project = _currentProjectRepository.FindCurrentProject();
      project.UnpackageIfNecessary();
      return CommandStatus.Success;
    }
  }
}