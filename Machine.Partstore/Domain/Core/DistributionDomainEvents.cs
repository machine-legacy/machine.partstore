using System;
using System.Collections.Generic;

using Machine.Partstore.Domain.Archiving;

namespace Machine.Partstore.Domain.Core
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
