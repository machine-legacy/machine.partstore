using System;
using System.Collections.Generic;

namespace DependencyStore.Domain.Repositories.Repositories
{
  public interface IRepositoryRepository
  {
    Repository FindDefaultRepository();
    void SaveRepository(Repository repository);
  }
}
