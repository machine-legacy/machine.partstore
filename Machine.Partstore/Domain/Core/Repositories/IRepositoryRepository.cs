using System;
using System.Collections.Generic;

using DependencyStore.Domain.FileSystem;

namespace DependencyStore.Domain.Core.Repositories
{
  public interface IRepositoryRepository
  {
    Repository FindRepository(Purl path);
    void SaveRepository(Repository repository);
    void RefreshRepository(Repository repository);
  }
}
