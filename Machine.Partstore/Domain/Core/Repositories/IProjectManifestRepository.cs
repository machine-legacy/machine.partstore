using System;
using System.Collections.Generic;

using Machine.Partstore.Domain.FileSystem;

namespace Machine.Partstore.Domain.Core.Repositories
{
  public interface IProjectManifestRepository
  {
    ProjectManifestStore FindProjectManifestStore(Purl path);
    void SaveProjectManifestStore(ProjectManifestStore projectManifestStore);
  }
}
