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
    private readonly IProjectRepository _projectRepository;
    private readonly IProjectManifestRepository _projectManifestRepository;
    private readonly DependencyStoreConfiguration _configuration;

    public SetManifestToLatestVersion(IProjectRepository projectRepository, IProjectManifestRepository projectManifestRepository, DependencyStoreConfiguration configuration)
    {
      _projectRepository = projectRepository;
      _projectManifestRepository = projectManifestRepository;
      _configuration = configuration;
    }

    public void SetAllProjects(Repository repository)
    {
      foreach (Project project in _projectRepository.FindAllProjects(_configuration))
      {
        foreach (ProjectManifest manifest in _projectManifestRepository.FindProjectManifests(project))
        {
          ArchivedProject archivedProject = repository.FindProject(manifest);
          if (archivedProject == null)
          {
            continue;
          }
          ArchivedProjectVersion version = archivedProject.LatestVersion;
          if (version == null)
          {
            continue;
          }
          ProjectManifest latestManifest = archivedProject.MakeManifest(version);
          Purl path = project.LibraryDirectory.Join(latestManifest.FileName);
          _projectManifestRepository.SaveProjectManifest(latestManifest, path);
        }
      }
    }
  }
}
