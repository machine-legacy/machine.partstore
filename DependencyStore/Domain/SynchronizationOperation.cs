using System;
using System.Collections.Generic;

namespace DependencyStore.Domain
{
  public class SynchronizationPlan : IEnumerable<SynchronizationOperation>
  {
    private readonly List<SynchronizationOperation> _operations = new List<SynchronizationOperation>();

    public void AddOperation(SynchronizationOperation operation)
    {
      _operations.Add(operation);
    }

    public void Merge(SynchronizationPlan plan)
    {
      _operations.AddRange(plan);
    }

    #region IEnumerable<SynchronizationOperation> Members
    public IEnumerator<SynchronizationOperation> GetEnumerator()
    {
      return _operations.GetEnumerator();
    }
    #endregion

    #region IEnumerable Members
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
    #endregion
  }
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