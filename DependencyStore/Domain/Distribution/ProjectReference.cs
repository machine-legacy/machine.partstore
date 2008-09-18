using System;
using System.Collections.Generic;

using DependencyStore.Domain.Core;

namespace DependencyStore.Domain.Distribution
{
  public class ProjectReference
  {
    private readonly Project _parentProject;
    private readonly ArchivedProject _dependency;
    private ArchivedProjectVersion _desiredVersion;

    public ProjectReference(Project parentProject, ArchivedProject dependency)
    {
      _parentProject = parentProject;
      _dependency = dependency;
      _desiredVersion = dependency.LatestVersion;
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
    }

    public void UnpackageIfNecessary()
    {
      UnpackagingDestination destination = this.UnpackagingDestination;
      if (destination.HasVersionOlderThan(_desiredVersion))
      {
        destination.UpdateInstalledVersion(_desiredVersion);
      }
    }

    private UnpackagingDestination UnpackagingDestination
    {
      get
      {
        ProjectManifest manifest = Infrastructure.ProjectManifestRepository.ReadProjectManifest(_parentProject.LibraryDirectory.Join(_dependency.Name + "." + ProjectManifest.Extension));
        return new UnpackagingDestination(_parentProject, manifest);
      }
    }
  }
}
