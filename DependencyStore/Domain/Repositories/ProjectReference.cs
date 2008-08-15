using System;
using System.Collections.Generic;

namespace DependencyStore.Domain.Repositories
{
  public class ProjectReference
  {
    private readonly Project _parentProject;
    private readonly ArchivedProject _dependency;
    private readonly ArchivedProjectVersion _version;
    private readonly ProjectManifest _manifest;

    public ProjectReference(ArchivedProject dependency, Project parentProject, ArchivedProjectVersion version, ProjectManifest manifest)
    {
      _dependency = dependency;
      _manifest = manifest;
      _parentProject = parentProject;
      _version = version;
    }

    public bool IsLatest
    {
      get { return false; }
    }

    public void MakeLatestVersion()
    {
      ProjectManifest latestManifest = _dependency.MakeManifest(_dependency.LatestVersion);
      Purl path = _parentProject.LibraryDirectory.Join(latestManifest.FileName);
      Infrastructure.ProjectManifestRepository.SaveProjectManifest(latestManifest, path);
    }
  }
}
