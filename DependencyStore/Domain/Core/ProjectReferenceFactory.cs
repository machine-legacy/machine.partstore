using System;
using System.Collections.Generic;

namespace DependencyStore.Domain.Core
{
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