using System;
using System.Collections.Generic;

using DependencyStore.Domain.Core;

namespace DependencyStore.Domain.Distribution
{
  public class ProjectReference
  {
    private readonly Project _parentProject;
    private readonly ArchivedProject _dependency;
    private readonly ArchivedProjectVersion _desiredVersion;
    private readonly UnpackagingDestination _destination;

    public ProjectReference(Project parentProject, ArchivedProject dependency, ArchivedProjectVersion desiredVersion)
    {
      _parentProject = parentProject;
      _dependency = dependency;
      _desiredVersion = desiredVersion;
      _destination = new UnpackagingDestination(_parentProject, _dependency);
    }

    private UnpackagingDestination UnpackagingDestination
    {
      get { return _destination; }
    }

    public ArchivedProject Dependency
    {
      get { return _dependency; }
    }

    public ArchivedProjectVersion DesiredVersion
    {
      get { return _desiredVersion; }
    }

    public bool IsDesiredVersionInstalled
    {
      get { return this.UnpackagingDestination.HasVersionOlderThan(this.DesiredVersion); }
    }

    public void UnpackageIfNecessary()
    {
      UnpackagingDestination destination = this.UnpackagingDestination;
      if (destination.HasVersionOlderThan(this.DesiredVersion))
      {
        destination.UpdateInstalledVersion(this.DesiredVersion);
      }
    }
  }
}
