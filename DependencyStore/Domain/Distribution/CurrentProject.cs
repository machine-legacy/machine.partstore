using System;
using System.Collections.Generic;

using DependencyStore.Domain.Core;
using DependencyStore.Domain.Services;

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

    public CurrentProject(string name, Purl rootDirectory, Purl buildDirectory, Purl libraryDirectory)
      : base(name, rootDirectory, buildDirectory, libraryDirectory)
    {
      _manifests = Infrastructure.ProjectManifestRepository.FindProjectManifestStore(this);
    }

    public ProjectReference AddReferenceToLatestVersion(ArchivedProject dependency)
    {
      _manifests.AddManifestFor(dependency, dependency.LatestVersion);
      return new ProjectReference(this, dependency, dependency.LatestVersion);
    }

    public void UnpackageIfNecessary()
    {
      foreach (ProjectReference reference in Infrastructure.ProjectReferenceRepository.FindAllProjectReferences())
      {
        reference.UnpackageIfNecessary();
      }
    }

    public void PublishNewVersion(Repository repository)
    {
      new AddingNewVersionsToRepository().PublishProject(this, repository);
    }
  }
}
