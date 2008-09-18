using System;
using System.Collections.Generic;

using DependencyStore.Domain.Core;

namespace DependencyStore.Domain.Repositories
{
  public class CurrentProject : Project
  {
    public CurrentProject(string name, Purl rootDirectory, Purl buildDirectory, Purl libraryDirectory)
      : base(name, rootDirectory, buildDirectory, libraryDirectory)
    {
    }

    public ProjectReference AddReferenceToLatestVersion(ArchivedProject project)
    {
      Console.WriteLine("Adding reference {0}", project);
      ProjectReference reference = Infrastructure.ProjectReferenceRepository.FindProjectReferenceFor(this, project);
      reference.MakeLatestVersion();
      return reference;
    }
  }
}
