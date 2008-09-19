using System;
using System.Collections.Generic;

namespace DependencyStore.Domain.Distribution
{
  public class ProjectReference
  {
    private readonly Project _parentProject;
    private readonly ArchivedProject _dependency;
    private readonly ArchivedProjectVersion _version;
    private readonly ProjectDependencyDirectory _installed;

    public ProjectReference(Project parentProject, ArchivedProject dependency, ArchivedProjectVersion version)
    {
      _parentProject = parentProject;
      _dependency = dependency;
      _version = version;
      _installed = new ProjectDependencyDirectory(_parentProject, _dependency);
    }

    private ProjectDependencyDirectory Installed
    {
      get { return _installed; }
    }

    public ArchivedProject Dependency
    {
      get { return _dependency; }
    }

    public ArchivedProjectVersion Version
    {
      get { return _version; }
    }

    public void UnpackageIfNecessary()
    {
      if (this.Status.IsOlderVersionInstalled)
      {
        this.Installed.UpdateInstalledVersion(this.Version);
      }
    }

    public ReferenceStatus Status
    {
      get
      {
        bool isAnyVersionInstalled = this.Installed.IsAnythingInstalled;
        bool isReferencedVersionInstalled = !this.Installed.HasVersionOlderThan(_version);
        bool isOlderVersionInstalled = this.Installed.HasVersionOlderThan(_version);
        bool isToLatestVersion = this.Dependency.LatestVersion == this.Version;
        return new ReferenceStatus(isToLatestVersion, isAnyVersionInstalled, isOlderVersionInstalled, isReferencedVersionInstalled);
      }
    }
  }
}
