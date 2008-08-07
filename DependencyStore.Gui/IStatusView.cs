using System;
using System.Collections.Generic;

using DependencyStore.Domain;

namespace DependencyStore.Gui
{
  public interface IStatusView
  {
    event EventHandler<EventArgs> SynchronizeAll;
    event EventHandler<EventArgs> Rescan;

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

    void Log(string message, params object[] args);
  }
}
