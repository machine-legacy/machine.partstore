using System;
using System.Collections.Generic;

using DependencyStore.Domain.Core;

namespace DependencyStore.Domain.Distribution.Repositories
{
  public interface IProjectManifestRepository
  {
    IList<ProjectManifest> FindProjectManifests(Project project);
    ProjectManifest ReadProjectManifest(Purl path);
    void SaveProjectManifest(ProjectManifest manifest, Purl path);
  }
}
