using System;
using DependencyStore.Domain.Archiving;
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
      ArchivingDomainEvents.Progress += OnProgress;
      CurrentProject project = _currentProjectRepository.FindCurrentProject();
      project.UnpackageIfNecessary();
      return CommandStatus.Success;
    }

    private static void OnProgress(object sender, ProgressEventArgs e)
    {
      ArchiveFileProgressEventArgs archiveArgs = (ArchiveFileProgressEventArgs)e;
      Console.Write("Unzipping {0:##.##}%\r", archiveArgs.PercentComplete * 100.0);
      if (e.PercentComplete == 1.0)
      {
        Console.WriteLine("Archive unpackaged: {0}", archiveArgs.Archive.Name);
      }
    }
  }
}