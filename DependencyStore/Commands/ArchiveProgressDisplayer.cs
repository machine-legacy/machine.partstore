using System;

using DependencyStore.Domain.Archiving;

namespace DependencyStore.Commands
{
  public class ArchiveProgressDisplayer
  {
    private readonly bool _packaging;

    public ArchiveProgressDisplayer(bool packaging)
    {
      _packaging = packaging;
      ArchivingDomainEvents.Progress += OnProgress;
    }

    private void OnProgress(object sender, ProgressEventArgs e)
    {
      ArchiveFileProgressEventArgs archiveArgs = (ArchiveFileProgressEventArgs)e;
      Console.Write(StringForStep, archiveArgs.PercentComplete * 100.0);
      if (e.PercentComplete == 1.0)
      {
        Console.WriteLine(StringForCompletion, archiveArgs.Archive.Name);
      }
    }

    private string StringForStep
    {
      get
      {
        return (_packaging ? "Packaging " : "Unpackaging ") + "{0:##.##}%\r";
      }
    }

    private string StringForCompletion
    {
      get { return _packaging ? "Archive packaged {0}" : "Archive unpackaged {0}"; }
    }
  }
}