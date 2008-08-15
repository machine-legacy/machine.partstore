using System;
using System.Collections.Generic;

using DependencyStore.Domain.Configuration;
using DependencyStore.Domain.Repositories;
using DependencyStore.Services.DataAccess;

namespace DependencyStore.Domain.Services
{
  [Machine.Container.Model.Transient]
  public class SetManifestToLatestVersion
  {
    private readonly IProjectReferenceRepository _projectReferenceRepository;
    private readonly DependencyStoreConfiguration _configuration;

    public SetManifestToLatestVersion(IProjectReferenceRepository projectReferenceRepository, DependencyStoreConfiguration configuration)
    {
      _projectReferenceRepository = projectReferenceRepository;
      _configuration = configuration;
    }

    public void SetAllProjects(Repository repository)
    {
      foreach (ProjectReference reference in _projectReferenceRepository.FindAllProjectReferences(_configuration))
      {
        reference.MakeLatestVersion();
      }
    }
  }
}
