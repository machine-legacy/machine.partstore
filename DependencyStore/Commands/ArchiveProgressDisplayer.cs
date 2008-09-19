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
      FileCopyProgressEventArgs copyArgs = (FileCopyProgressEventArgs)e;
      Console.Write(StringForStep, copyArgs.PercentComplete * 100.0);
      if (e.PercentComplete == 1.0)
      {
        Console.WriteLine(StringForCompletion, copyArgs.Destiny.Name);
      }
    }

    private string StringForStep
    {
      get
      {
        return (_packaging ? "Committing " : "Checking out ") + "{0:##.##}%\r";
      }
    }

    private string StringForCompletion
    {
      get { return _packaging ? "Committed {0}" : "Checking out {0}"; }
    }
  }
}