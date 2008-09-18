using System;
using System.Collections.Generic;

using DependencyStore.Domain.Distribution;
using DependencyStore.Domain.Distribution.Repositories;

namespace DependencyStore.Domain.Services
{
  [Machine.Container.Model.Transient]
  public class UnpackagingDependenciesForProjects
  {
    private readonly IProjectReferenceRepository _projectReferenceRepository;

    public UnpackagingDependenciesForProjects(IProjectReferenceRepository projectReferenceRepository)
    {
      _projectReferenceRepository = projectReferenceRepository;
    }

    public void Unpackage(Repository repository)
    {
      foreach (ProjectReference reference in _projectReferenceRepository.FindAllProjectReferences())
      {
        reference.InstallPackageIfNecessary();
      }
    }
  }
}
