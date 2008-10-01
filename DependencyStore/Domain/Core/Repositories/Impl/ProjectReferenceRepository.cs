using System;
using System.Collections.Generic;

namespace DependencyStore.Domain.Core.Repositories.Impl
{
  public class ProjectReferenceRepository : IProjectReferenceRepository
  {
    private readonly IRepositorySetRepository _repositorySetRepository;
    private readonly IProjectManifestRepository _projectManifestRepository;

    public ProjectReferenceRepository(IRepositorySetRepository repositorySetRepository, IProjectManifestRepository projectManifestRepository)
    {
      _repositorySetRepository = repositorySetRepository;
      _projectManifestRepository = projectManifestRepository;
    }

    public IList<ProjectReference> FindProjectReferences(Project project)
    {
      RepositorySet repositorySet = _repositorySetRepository.FindDefaultRepositorySet();
      ProjectManifestStore manifestStore = _projectManifestRepository.FindProjectManifestStore(project);
      List<ProjectReference> references = new List<ProjectReference>();
      references.AddRange(ProjectReferenceFactory.FindProjectReferences(repositorySet, project, manifestStore));
      return references;
    }
  }
  public class ProjectReferenceFactory
  {
    public static IEnumerable<ProjectReference> FindProjectReferences(RepositorySet repositorySet, Project project, ProjectManifestStore manifestStore)
    {
      foreach (ProjectManifest manifest in manifestStore)
      {
        ArchivedProject archivedProject = repositorySet.FindProject(manifest.ProjectName);
        if (archivedProject == null)
        {
          yield return BrokenProjectReference.MissingProject(manifest);
          continue;
        }
        ArchivedProjectVersion version = archivedProject.FindVersionInManifest(manifest);
        if (version == null)
        {
          yield return BrokenProjectReference.MissingVersion(manifest);
          continue;
        }
        ArchivedProjectAndVersion archivedProjectAndVersion = new ArchivedProjectAndVersion(archivedProject, version);
        yield return new HealthyProjectReference(project, archivedProjectAndVersion);
      }
    }
  }
}
