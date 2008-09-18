using System;
using System.Collections.Generic;

using DependencyStore.Domain.Core;

namespace DependencyStore.Domain.SimpleCopying
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

    public bool IsEmpty
    {
      get { return _operations.Count == 0; }
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
}