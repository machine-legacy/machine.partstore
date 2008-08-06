using System;
using System.Collections.Generic;

using DependencyStore.Domain;

namespace DependencyStore.Gui
{
  public interface IStatusView
  {
    LatestFileSet LatestFiles
    {
      get;
      set;
    }

    SynchronizationPlan SynchronizationPlan
    {
      get;
      set;
    }
  }
}
