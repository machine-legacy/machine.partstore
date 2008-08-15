using System;
using System.Collections.Generic;
using DependencyStore.Domain;

namespace DependencyStore.Services.DataAccess
{
  public interface IProjectManifestRepository
  {
    IList<ProjectManifest> FindProjectManifests(Project project);
    ProjectManifest ReadProjectManifest(Purl path);
    void SaveProjectManifest(ProjectManifest manifest, Purl path);
  }
}
