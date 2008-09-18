using System;
using System.Collections;
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

    public IEnumerable References
    {
      get { return Infrastructure.ProjectReferenceRepository.FindAllProjectReferences(); }
    }

    public ProjectReference AddReferenceToLatestVersion(ArchivedProject project)
    {
      Console.WriteLine("Adding reference {0}", project);
      ProjectReference reference = Infrastructure.ProjectReferenceRepository.FindProjectReferenceFor(this, project);
      reference.MakeLatestVersion();
      return reference;
    }

    public void InstallPackagesIfNecessary()
    {
      foreach (ProjectReference reference in Infrastructure.ProjectReferenceRepository.FindAllProjectReferences())
      {
        reference.InstallPackageIfNecessary();
      }
    }

    public void PublishNewVersion(Repository repository)
    {
      new AddingNewVersionsToRepository().PublishProject(this, repository);
    }
  }
}
