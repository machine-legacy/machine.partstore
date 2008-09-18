using System;
using System.Collections.Generic;

using DependencyStore.Domain.Core;

namespace DependencyStore.Domain.Repositories
{
  public class ProjectReference
  {
    private readonly Project _parentProject;
    private readonly ArchivedProject _dependency;
    private ProjectManifest _manifest;
    private ArchivedProjectVersion _desiredVersion;

    public ProjectReference(Project parentProject, ArchivedProject dependency)
    {
      _parentProject = parentProject;
      _dependency = dependency;
      _desiredVersion = dependency.LatestVersion;
      _manifest = null;
    }

    public ProjectReference(Project parentProject, ArchivedProject dependency, ArchivedProjectVersion desiredVersion, ProjectManifest manifest)
    {
      _parentProject = parentProject;
      _dependency = dependency;
      _desiredVersion = desiredVersion;
      _manifest = manifest;
    }

    public Project ParentProject
    {
      get { return _parentProject; }
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
      get { return this.UnpackagingDestination.HasVersionOlderThan(_desiredVersion); }
    }

    public void MakeLatestVersion()
    {
      ProjectManifest latestManifest = _dependency.MakeManifestForLatestVersion();
      Purl path = _parentProject.LibraryDirectory.Join(latestManifest.FileName);
      Infrastructure.ProjectManifestRepository.SaveProjectManifest(latestManifest, path);
      _desiredVersion = _dependency.LatestVersion;
      _manifest = latestManifest;
    }

    public void InstallPackageIfNecessary()
    {
      UnpackagingDestination destination = this.UnpackagingDestination;
      if (destination.HasVersionOlderThan(_desiredVersion))
      {
        destination.UpdateInstalledVersion(_desiredVersion);
      }
    }

    private UnpackagingDestination UnpackagingDestination
    {
      get { return new UnpackagingDestination(_parentProject, _manifest); }
    }
  }
}
