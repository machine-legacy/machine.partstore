using System;
using System.Collections.Generic;

namespace DependencyStore.Domain.Core.Repositories
{
  public interface IRepositoryRepository
  {
    Repository FindDefaultRepository();
    void SaveRepository(Repository repository);
    void RefreshRepository(Repository repository);
  }
}
