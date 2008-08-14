using System;
using System.Collections.Generic;

using DependencyStore.Domain;
using DependencyStore.Domain.Configuration;

namespace DependencyStore.Services.DataAccess.Impl
{
  public class RepositoryRepository : IRepositoryRepository
  {
    #region IRepositoryRepository Members
    public Repository FindDefaultRepository(DependencyStoreConfiguration configuration)
    {
      Repository repository = new Repository();
      return repository;
    }
    #endregion
  }
}
