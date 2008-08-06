using System;

namespace DependencyStore.Domain
{
  public abstract class SynchronizationOperation
  {
  }
  public class UpdateOutOfDateFile : SynchronizationOperation
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

    public UpdateOutOfDateFile(SinkLocation sinkLocation, FileAsset sinkFile, FileAsset sourceFile)
    {
      _sinkLocation = sinkLocation;
      _sinkFile = sinkFile;
      _sourceFile = sourceFile;
    }
  }
}