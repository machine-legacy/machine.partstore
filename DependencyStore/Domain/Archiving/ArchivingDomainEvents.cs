using System;
using System.Collections.Generic;
using DependencyStore.Domain.Core;

namespace DependencyStore.Domain.Archiving
{
  public class ArchivingDomainEvents
  {
    public static event EventHandler<ProgressEventArgs> Progress;

    public static void OnProgress(object sender, ProgressEventArgs e)
    {
      if (Progress == null) return;
      Progress(sender, e);
    }
  }
  public class ProgressEventArgs : EventArgs
  {
    private readonly double _percentComplete;

    public double PercentComplete
    {
      get { return _percentComplete; }
    }

    public ProgressEventArgs(double percentComplete)
    {
      _percentComplete = percentComplete;
    }
  }
  public class ArchiveFileProgressEventArgs : ProgressEventArgs
  {
    private readonly ManifestEntry _manifestEntry;
    private readonly Purl _archive;

    public Purl Archive
    {
      get { return _archive; }
    }

    public ManifestEntry ManifestEntry
    {
      get { return _manifestEntry; }
    }

    public ArchiveFileProgressEventArgs(double percentComplete, Purl archive, ManifestEntry manifestEntry)
     : base(percentComplete)
    {
      _archive = archive;
      _manifestEntry = manifestEntry;
    }
  }
}
