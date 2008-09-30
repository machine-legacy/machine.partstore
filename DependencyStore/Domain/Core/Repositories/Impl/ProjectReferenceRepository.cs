using System;
using System.Collections.Generic;

namespace DependencyStore.Domain.Core.Repositories.Impl
{
  public class ProjectReferenceRepository : IProjectReferenceRepository
  {
    private readonly IRepositorySetRepository _repositorySetRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IProjectManifestRepository _projectManifestRepository;

    public ProjectReferenceRepository(IRepositorySetRepository repositorySetRepository, IProjectRepository projectRepository, IProjectManifestRepository projectManifestRepository)
    {
      _repositorySetRepository = repositorySetRepository;
      _projectRepository = projectRepository;
      _projectManifestRepository = projectManifestRepository;
    }

    public IList<ProjectReference> FindAllProjectReferences()
    {
      RepositorySet repositorySet = _repositorySetRepository.FindDefaultRepositorySet();
      List<ProjectReference> references = new List<ProjectReference>();
      foreach (Project project in _projectRepository.FindAllProjects())
      {
        ProjectManifestStore manifestStore = _projectManifestRepository.FindProjectManifestStore(project);
        foreach (ProjectManifest manifest in manifestStore)
        {
          ArchivedProject archivedProject = repositorySet.FindProject(manifest.ProjectName);
          if (archivedProject == null)
          {
            references.Add(BrokenProjectReference.MissingProject(manifest));
            continue;
          }
          ArchivedProjectVersion version = archivedProject.FindVersionInManifest(manifest);
          if (version == null)
          {
            references.Add(BrokenProjectReference.MissingVersion(manifest));
            continue;
          }
          references.Add(new HealthyProjectReference(project, archivedProject, version));
        }
      }
      return references;
    }
  }
}
