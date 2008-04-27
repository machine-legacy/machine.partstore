using System;
using System.Collections.Generic;

using DependencyStore.Domain;

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
}
