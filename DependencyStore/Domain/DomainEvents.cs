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
}
