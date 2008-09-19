using System;
using System.Collections.Generic;

using DependencyStore.Domain.Core;

namespace DependencyStore.Domain.Distribution
{
  public class CurrentProject : Project
  {
    private readonly ProjectManifestStore _manifests;

    public ProjectManifestStore Manifests
    {
      get { return _manifests; }
    }

    public IEnumerable<ProjectReference> References
    {
      get { return Infrastructure.ProjectReferenceRepository.FindAllProjectReferences(); }
    }

    public CurrentProject(string name, Purl rootDirectory, Purl buildDirectory, Purl libraryDirectory, ProjectManifestStore manifests)
      : base(name, rootDirectory, buildDirectory, libraryDirectory)
    {
      _manifests = manifests;
    }

    public ProjectReference AddReferenceToLatestVersion(ArchivedProject dependency)
    {
      _manifests.AddManifestFor(dependency, dependency.LatestVersion);
      return new ProjectReference(this, dependency, dependency.LatestVersion);
    }

    public void UnpackageIfNecessary(Repository repository)
    {
      foreach (ProjectReference reference in Infrastructure.ProjectReferenceRepository.FindAllProjectReferences())
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
