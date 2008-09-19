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
    private readonly ProjectDependencyDirectory _dependencyDirectory;

    public ProjectReference(Project parentProject, ArchivedProject dependency, ArchivedProjectVersion desiredVersion)
    {
      _parentProject = parentProject;
      _dependency = dependency;
      _desiredVersion = desiredVersion;
      _dependencyDirectory = new ProjectDependencyDirectory(_parentProject, _dependency);
    }

    private ProjectDependencyDirectory ProjectDependencyDirectory
    {
      get { return _dependencyDirectory; }
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
      get { return this.ProjectDependencyDirectory.HasVersionOlderThan(this.DesiredVersion); }
    }

    public void UnpackageIfNecessary()
    {
      ProjectDependencyDirectory dependencyDirectory = this.ProjectDependencyDirectory;
      if (dependencyDirectory.HasVersionOlderThan(this.DesiredVersion))
      {
        dependencyDirectory.UpdateInstalledVersion(this.DesiredVersion);
      }
    }
  }
}
