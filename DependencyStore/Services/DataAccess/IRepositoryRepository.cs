using System;
using System.Collections.Generic;

using DependencyStore.Domain.Repositories;

namespace DependencyStore.Services.DataAccess
{
  public interface IRepositoryRepository
  {
    Repository FindDefaultRepository();
    void SaveRepository(Repository repository);
  }
}
