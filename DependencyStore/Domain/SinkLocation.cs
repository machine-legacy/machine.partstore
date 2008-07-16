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

    public SinkLocation(FileSystemPath path, FileSystemEntry entry)
      : base(path, entry)
    {
    }

    public SinkLocation()
    {
    }

    public void CheckForNewerFiles(LatestFileSet latestFileSet)
    {
      foreach (FileSystemFile file in this.FileEntry.BreadthFirstFiles)
      {
        FileSystemFile possiblyNewer = latestFileSet.FindExistingByName(file);
        if (possiblyNewer != null && possiblyNewer.IsNewerThan(file))
        {
          DomainEvents.OnEncounteredOutdatedSinkFile(this, new OutdatedSinkFileEventArgs(this, file, possiblyNewer));
        }
      }
    }
  }
}