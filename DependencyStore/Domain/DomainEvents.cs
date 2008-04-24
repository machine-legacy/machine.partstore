using System;
using System.Collections.Generic;

namespace DependencyStore.Domain
{
  public static class DomainEvents
  {
    public static event EventHandler<OutdatedSinkFileEventArgs> EncounteredOutdatedSinkFile;
  }
  public class OutdatedSinkFileEventArgs : EventArgs
  {
    private readonly FileSystemFile _sourceFile;
    private readonly FileSystemFile _sinkFile;

    public FileSystemFile SourceFile
    {
      get { return _sourceFile; }
    }

    public FileSystemFile SinkFile
    {
      get { return _sinkFile; }
    }

    public OutdatedSinkFileEventArgs(FileSystemFile sinkFile, FileSystemFile sourceFile)
    {
      _sinkFile = sinkFile;
      _sourceFile = sourceFile;
    }
  }
}
