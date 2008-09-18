using System;
using System.Collections.Generic;

using DependencyStore.Domain.Core;

namespace DependencyStore.Domain.SimpleCopying
{
  public class SinkLocation : Location
  {
    public override bool IsSource
    {
      get { return false; }
    }

    public SinkLocation(Purl path, FileSystemEntry entry)
      : base(path, entry)
    {
    }

    public SinkLocation()
    {
    }

    public SynchronizationPlan CreateSynchronizationPlan(LatestFileSet latestFiles)
    {
      SynchronizationPlan plan = new SynchronizationPlan();
      foreach (FileSystemFile file in this.FileEntry.BreadthFirstFiles)
      {
        FileAsset possiblyNewer = latestFiles.FindExistingByName(file);
        if (possiblyNewer != null && possiblyNewer.IsNewerThan(file))
        {
          plan.AddOperation(new UpdateOutOfDateFile(this, file, possiblyNewer));
        }
      }
      return plan;
    }
  }
}