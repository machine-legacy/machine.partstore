using System;
using System.Collections.Generic;

using DependencyStore.Domain.Archiving;

namespace DependencyStore.Domain.Distribution
{
  public class DistributionDomainEvents
  {
    public static event EventHandler<FileCopyProgressEventArgs> FileCopyProgress;

    public static void OnProgress(object sender, FileCopyProgressEventArgs e)
    {
      if (FileCopyProgress == null) return;
      FileCopyProgress(sender, e);
    }
  }
}
