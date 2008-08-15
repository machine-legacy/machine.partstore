using System;
using System.Collections.Generic;

using DependencyStore.Domain.Configuration;
using DependencyStore.Domain.Repositories;

namespace DependencyStore.Services.DataAccess
{
  public interface IRepositoryRepository
  {
    Repository FindDefaultRepository(DependencyStoreConfiguration configuration);
    void SaveRepository(Repository repository, DependencyStoreConfiguration configuration);
  }
}
