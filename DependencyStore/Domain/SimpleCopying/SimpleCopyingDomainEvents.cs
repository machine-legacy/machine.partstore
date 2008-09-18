using System;
using System.Collections.Generic;

using DependencyStore.Domain.Core;
using DependencyStore.Domain.Archiving;

namespace DependencyStore.Domain.SimpleCopying
{
  public static class SimpleCopyingDomainEvents
  {
    public static event EventHandler<LocationNotFoundEventArgs> LocationNotFound;

    public static void OnLocationNotFound(object sender, LocationNotFoundEventArgs e)
    {
      if (LocationNotFound == null) return;
      LocationNotFound(sender, e);
    }
  }
  public class LocationNotFoundEventArgs : EventArgs
  {
    private readonly Purl _path;

    public Purl Path
    {
      get { return _path; }
    }

    public LocationNotFoundEventArgs(Purl path)
    {
      _path = path;
    }
  }
}
