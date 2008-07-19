using System;
using System.Collections.Generic;

using DependencyStore.Domain;
using DependencyStore.Domain.Archiving;

namespace DependencyStore.Domain
{
  public static class DomainEvents
  {
    public static event EventHandler<OutdatedSinkFileEventArgs> EncounteredOutdatedSinkFile;

    public static void OnEncounteredOutdatedSinkFile(object sender, OutdatedSinkFileEventArgs e)
    {
      if (EncounteredOutdatedSinkFile == null) return;
      EncounteredOutdatedSinkFile(sender, e);
    }

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
  public class OutdatedSinkFileEventArgs : EventArgs
  {
    private readonly SinkLocation _sinkLocation;
    private readonly FileAsset _sourceFile;
    private readonly FileAsset _sinkFile;

    public SinkLocation SinkLocation
    {
      get { return _sinkLocation; }
    }

    public FileAsset SourceFile
    {
      get { return _sourceFile; }
    }

    public FileAsset SinkFile
    {
      get { return _sinkFile; }
    }

    public OutdatedSinkFileEventArgs(SinkLocation sinkLocation, FileAsset sinkFile, FileAsset sourceFile)
    {
      _sinkLocation = sinkLocation;
      _sinkFile = sinkFile;
      _sourceFile = sourceFile;
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
  public class ZipFileProgressEventArgs : ProgressEventArgs
  {
    private readonly ManifestEntry _zipFileEntry;

    public ManifestEntry ZipFileEntry
    {
      get { return _zipFileEntry; }
    }

    public ZipFileProgressEventArgs(double percentComplete, ManifestEntry zipFileEntry)
     : base(percentComplete)
    {
      _zipFileEntry = zipFileEntry;
    }
  }
}
