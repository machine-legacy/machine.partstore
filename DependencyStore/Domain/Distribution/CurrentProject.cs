using System;
using System.Collections.Generic;

using DependencyStore.Domain.Core;
using DependencyStore.Domain.Services;

namespace DependencyStore.Domain.Distribution
{
  public class CurrentProject : Project
  {
    private readonly List<ProjectReference> _references = new List<ProjectReference>();

    public CurrentProject(string name, Purl rootDirectory, Purl buildDirectory, Purl libraryDirectory)
      : base(name, rootDirectory, buildDirectory, libraryDirectory)
    {
    }

    public IEnumerable<ProjectReference> References
    {
      get { return Infrastructure.ProjectReferenceRepository.FindAllProjectReferences(); }
    }

    public ProjectReference AddReferenceToLatestVersion(ArchivedProject project)
    {
      ProjectReference reference = Infrastructure.ProjectReferenceRepository.FindProjectReferenceFor(this, project);
      reference.MakeLatestVersion();
      _references.Add(reference);
      return reference;
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
