using System;
using System.Collections.Generic;

using DependencyStore.Domain.Core;
using DependencyStore.Domain.Services;

namespace DependencyStore.Domain.Distribution
{
  public class CurrentProject : Project
  {
    public CurrentProject(string name, Purl rootDirectory, Purl buildDirectory, Purl libraryDirectory)
      : base(name, rootDirectory, buildDirectory, libraryDirectory)
    {
    }

    public IEnumerable<ProjectReference> References
    {
      get { return Infrastructure.ProjectReferenceRepository.FindAllProjectReferences(); }
    }

    public ProjectReference AddReferenceToLatestVersion(ArchivedProject dependency)
    {
      ProjectManifest latestManifest = dependency.MakeManifestForLatestVersion();
      Purl path = this.LibraryDirectory.Join(dependency.ManifestFileName);
      Infrastructure.ProjectManifestRepository.SaveProjectManifest(latestManifest, path);
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
