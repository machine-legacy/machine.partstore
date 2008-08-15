using System;
using System.Collections.Generic;

using DependencyStore.Domain.Archiving;
using DependencyStore.Domain.Configuration;
using DependencyStore.Services.DataAccess;

namespace DependencyStore.Domain.Services
{
  [Machine.Container.Model.Transient]
  public class UnpackageProjectManifest
  {
    private readonly IProjectRepository _projectRepository;
    private readonly IProjectManifestRepository _projectManifestRepository;
    private readonly DependencyStoreConfiguration _configuration;

    public UnpackageProjectManifest(IProjectRepository projectRepository, IProjectManifestRepository projectManifestRepository, DependencyStoreConfiguration configuration)
    {
      _projectRepository = projectRepository;
      _projectManifestRepository = projectManifestRepository;
      _configuration = configuration;
    }

    public void Unpackage(Repository repository)
    {
      foreach (Project project in _projectRepository.FindAllProjects(_configuration))
      {
        foreach (ProjectManifest manifest in _projectManifestRepository.FindProjectManifests(project))
        {
          ArchivedProjectVersion version = repository.FindProjectVersion(manifest);
          if (version == null)
          {
            continue;
          }
          UnpackagingDestination destination = new UnpackagingDestination(_configuration, project, manifest);
          if (destination.HasVersionOlderThan(version))
          {
            destination.UpdateToNewVersion(version);
          }
        }
      }
    }
  }
}
