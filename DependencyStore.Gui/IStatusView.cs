using System;
using System.Collections.Generic;

using DependencyStore.Domain.SimpleCopying;

namespace DependencyStore.Gui
{
  public interface IStatusView
  {
    event EventHandler<EventArgs> SynchronizeAll;
    event EventHandler<LocationEventArgs> Synchronize;
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
  public class LocationEventArgs : EventArgs
  {
    private readonly Location _location;

    public Location Location
    {
      get { return _location; }
    }

    public LocationEventArgs(Location location)
    {
      _location = location;
    }
  }
}
