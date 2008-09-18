using System;
using System.Collections.Generic;

using DependencyStore.Domain.Core;

namespace DependencyStore.Domain.Distribution
{
  public class ProjectReference
  {
    private readonly Project _parentProject;
    private readonly ArchivedProject _dependency;
    private readonly ProjectManifest _manifest;
    private readonly UnpackagingDestination _destination;
    private ArchivedProjectVersion _dependencyVersion;

    public ProjectReference(Project parentProject, ArchivedProject dependency, ProjectManifest manifest)
    {
      _parentProject = parentProject;
      _dependency = dependency;
      _manifest = manifest;
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

    public ArchivedProjectVersion DependencyVersion
    {
      get { throw new NotImplementedException(); }
    }

    public bool IsDesiredVersionInstalled
    {
      get { return this.UnpackagingDestination.HasVersionOlderThan(this.DependencyVersion); }
    }

    public void UnpackageIfNecessary()
    {
      UnpackagingDestination destination = this.UnpackagingDestination;
      if (destination.HasVersionOlderThan(this.DependencyVersion))
      {
        destination.UpdateInstalledVersion(this.DependencyVersion);
      }
    }
  }
}
