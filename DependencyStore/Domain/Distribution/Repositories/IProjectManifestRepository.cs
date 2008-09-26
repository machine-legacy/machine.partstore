using System;
using System.Collections.Generic;

using DependencyStore.Domain.FileSystem;

namespace DependencyStore.Domain.Distribution.Repositories
{
  public interface IProjectManifestRepository
  {
    ProjectManifestStore FindProjectManifestStore(Purl path);
    ProjectManifestStore FindProjectManifestStore(Project project);
    void SaveProjectManifestStore(ProjectManifestStore projectManifestStore);
  }
}
