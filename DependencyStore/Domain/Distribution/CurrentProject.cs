using System;
using System.Collections.Generic;

using DependencyStore.Domain.Core;

namespace DependencyStore.Domain.Distribution
{
  public class CurrentProject : Project
  {
    private readonly ProjectManifestStore _manifests;
    private readonly IList<ProjectReference> _references;

    public ProjectManifestStore Manifests
    {
      get { return _manifests; }
    }

    public IEnumerable<ReferenceStatus> ReferenceStatuses
    {
      get
      {
        foreach (ProjectReference reference in _references)
        {
          yield return reference.Status;
        }
      }
    }

    public CurrentProject(string name, Purl rootDirectory, Purl buildDirectory, Purl libraryDirectory, ProjectManifestStore manifests)
      : base(name, rootDirectory, buildDirectory, libraryDirectory)
    {
      _manifests = manifests;
      _references = Infrastructure.ProjectReferenceRepository.FindAllProjectReferences();
    }

    public ProjectReference AddReferenceToLatestVersion(ArchivedProject dependency)
    {
      _manifests.AddManifestFor(dependency, dependency.LatestVersion);
      return new HealthyProjectReference(this, dependency, dependency.LatestVersion);
    }

    public void UnpackageIfNecessary(Repository repository)
    {
      foreach (ProjectReference reference in _references)
      {
        reference.UnpackageIfNecessary(repository);
      }
    }

    public void AddNewVersion(Repository repository)
    {
      repository.AddNewVersion(this);
    }
  }
}
