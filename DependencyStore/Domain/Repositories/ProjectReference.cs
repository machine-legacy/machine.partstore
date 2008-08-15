using System;
using System.Collections.Generic;
using DependencyStore.Domain.Configuration;

namespace DependencyStore.Domain.Repositories
{
  public class ProjectReference
  {
    private readonly DependencyStoreConfiguration _configuration;
    private readonly Project _parentProject;
    private readonly ArchivedProject _dependency;
    private ProjectManifest _manifest;
    private ArchivedProjectVersion _desiredVersion;

    public ProjectReference(DependencyStoreConfiguration configuration, ArchivedProject dependency, Project parentProject, ArchivedProjectVersion desiredVersion, ProjectManifest manifest)
    {
      _configuration = configuration;
      _dependency = dependency;
      _manifest = manifest;
      _parentProject = parentProject;
      _desiredVersion = desiredVersion;
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

    public bool IsUsingOlderVersion
    {
      get { return _manifest.IsOlderThan(_dependency.LatestVersion); }
    }

    public bool IsDesiredVersionInstalled
    {
      get { return this.UnpackagingDestination.HasVersionOlderThan(_desiredVersion); }
    }

    public void MakeLatestVersion()
    {
      ArchivedProjectVersion latestVersion = _dependency.LatestVersion;
      ProjectManifest latestManifest = _dependency.MakeManifest(latestVersion);
      Purl path = _parentProject.LibraryDirectory.Join(latestManifest.FileName);
      Infrastructure.ProjectManifestRepository.SaveProjectManifest(latestManifest, path);
      _desiredVersion = latestVersion;
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
      get { return new UnpackagingDestination(_configuration, _parentProject, _manifest); }
    }
  }
}
