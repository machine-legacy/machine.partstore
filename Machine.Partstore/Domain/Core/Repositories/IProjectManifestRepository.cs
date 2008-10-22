using System;
using System.Collections.Generic;

using DependencyStore.Domain.FileSystem;

namespace DependencyStore.Domain.Core.Repositories
{
  public interface IProjectManifestRepository
  {
    ProjectManifestStore FindProjectManifestStore(Purl path);
    void SaveProjectManifestStore(ProjectManifestStore projectManifestStore);
  }
}
