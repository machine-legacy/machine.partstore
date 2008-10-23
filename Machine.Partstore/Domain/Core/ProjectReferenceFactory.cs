using System;
using System.Collections.Generic;

namespace Machine.Partstore.Domain.Core
{
  public class ProjectReferenceFactory
  {
    public static IEnumerable<ProjectReference> FindProjectReferences(RepositorySet repositorySet, Project project, ProjectManifestStore manifestStore)
    {
      foreach (ProjectManifest manifest in manifestStore)
      {
        ProjectFromRepository projectFromRepository = repositorySet.FindProject(manifest.ProjectName);
        if (projectFromRepository == null)
        {
          yield return BrokenProjectReference.MissingProject(manifest);
          continue;
        }
        ArchivedProjectVersion version = projectFromRepository.Project.FindVersionInManifest(manifest);
        if (version == null)
        {
          yield return BrokenProjectReference.MissingVersion(manifest);
          continue;
        }
        ArchivedProjectAndVersion archivedProjectAndVersion = new ArchivedProjectAndVersion(projectFromRepository, version);
        yield return new HealthyProjectReference(project, archivedProjectAndVersion);
      }
    }
  }
}