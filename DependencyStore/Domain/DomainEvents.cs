using System;
using System.Collections.Generic;

using DependencyStore.Domain;
using DependencyStore.Domain.Archiving;

namespace DependencyStore.Domain
{
  public static class DomainEvents
  {
    public static event EventHandler<LocationNotFoundEventArgs> LocationNotFound;

    public static void OnLocationNotFound(object sender, LocationNotFoundEventArgs e)
    {
      if (LocationNotFound == null) return;
      LocationNotFound(sender, e);
    }

    public static event EventHandler<ProgressEventArgs> Progress;

    public static void OnProgress(object sender, ProgressEventArgs e)
    {
      if (Progress == null) return;
      Progress(sender, e);
    }
  }
  public class LocationNotFoundEventArgs : EventArgs
  {
    private readonly Purl _path;

    public Purl Path
    {
      get { return _path; }
    }

    public LocationNotFoundEventArgs(Purl path)
    {
      _path = path;
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

    public ManifestEntry ManifestEntry
    {
      get { return _manifestEntry; }
    }

    public ArchiveFileProgressEventArgs(double percentComplete, ManifestEntry manifestEntry)
     : base(percentComplete)
    {
      _manifestEntry = manifestEntry;
    }
  }
}
