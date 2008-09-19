using System;
using System.Collections.Generic;

using DependencyStore.Domain.Core;
using DependencyStore.Domain.Core.Repositories;

namespace DependencyStore.Domain.Distribution.Repositories.Impl
{
  public class ProjectReferenceRepository : IProjectReferenceRepository
  {
    private readonly IRepositoryRepository _repositoryRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IProjectManifestRepository _projectManifestRepository;

    public ProjectReferenceRepository(IRepositoryRepository repositoryRepository, IProjectRepository projectRepository, IProjectManifestRepository projectManifestRepository)
    {
      _repositoryRepository = repositoryRepository;
      _projectRepository = projectRepository;
      _projectManifestRepository = projectManifestRepository;
    }

    public IList<ProjectReference> FindAllProjectReferences()
    {
      Repository repository = _repositoryRepository.FindDefaultRepository();
      List<ProjectReference> references = new List<ProjectReference>();
      foreach (Project project in _projectRepository.FindAllProjects())
      {
        ProjectManifestStore manifestStore = _projectManifestRepository.FindProjectManifestStore(project);
        foreach (ProjectManifest manifest in manifestStore)
        {
          ArchivedProject archivedProject = repository.FindProject(manifest);
          if (archivedProject == null)
          {
            throw new ArchivedProjectNotFoundException("Missing project: " + manifest);
          }
          ArchivedProjectVersion version = archivedProject.FindVersionInManifest(manifest);
          if (version == null)
          {
            throw new ArchivedVersionNotFoundException("Missing version: " + manifest);
          }
          ProjectReference reference = new ProjectReference(project, archivedProject, version);
          references.Add(reference);
        }
      }
      return references;
    }
  }
}
