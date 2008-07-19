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
    private readonly FileSystemFile _sourceFile;
    private readonly FileSystemFile _sinkFile;

    public SinkLocation SinkLocation
    {
      get { return _sinkLocation; }
    }

    public FileSystemFile SourceFile
    {
      get { return _sourceFile; }
    }

    public FileSystemFile SinkFile
    {
      get { return _sinkFile; }
    }

    public OutdatedSinkFileEventArgs(SinkLocation sinkLocation, FileSystemFile sinkFile, FileSystemFile sourceFile)
    {
      _sinkLocation = sinkLocation;
      _sinkFile = sinkFile;
      _sourceFile = sourceFile;
    }
  }
  public class LocationNotFoundEventArgs : EventArgs
  {
    private readonly FileSystemPath _path;

    public FileSystemPath Path
    {
      get { return _path; }
    }

    public LocationNotFoundEventArgs(FileSystemPath path)
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
  public class ZipFileProgressEventArgs : ProgressEventArgs
  {
    private readonly ManifestEntry _manifestEntry;

    public ManifestEntry ManifestEntry
    {
      get { return _manifestEntry; }
    }

    public ZipFileProgressEventArgs(double percentComplete, ManifestEntry manifestEntry)
     : base(percentComplete)
    {
      _manifestEntry = manifestEntry;
    }
  }
}
