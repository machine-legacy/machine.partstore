using System;
using System.Collections.Generic;

namespace DependencyStore.Domain
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

    public IEnumerable<SynchronizationOperation> CreateSynchronizationPlan(LatestFileSet latestFiles)
    {
      foreach (FileSystemFile file in this.FileEntry.BreadthFirstFiles)
      {
        FileAsset possiblyNewer = latestFiles.FindExistingByName(file);
        if (possiblyNewer != null && possiblyNewer.IsNewerThan(file))
        {
          yield return new UpdateOutOfDateFile(this, file, possiblyNewer);
        }
      }
    }

    public void CheckForNewerFiles(LatestFileSet latestFiles)
    {
      foreach (UpdateOutOfDateFile update in CreateSynchronizationPlan(latestFiles))
      {
        DomainEvents.OnEncounteredOutdatedSinkFile(this, new OutdatedSinkFileEventArgs(update));
      }
    }
  }
}