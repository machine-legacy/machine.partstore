using System;
using System.Collections.Generic;

using DependencyStore.Domain;

namespace DependencyStore.Gui
{
  public interface IStatusView
  {
    FileSetGroupedByLocation LatestFiles
    {
      get;
      set;
    }

    SynchronizationPlan SynchronizationPlan
    {
      get;
      set;
    }

    event EventHandler<EventArgs> Synchronize;
  }
}
