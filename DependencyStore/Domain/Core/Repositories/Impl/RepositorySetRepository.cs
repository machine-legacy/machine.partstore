using System;
using System.Collections.Generic;

namespace DependencyStore.Domain.Core.Repositories.Impl
{
  public class RepositorySetRepository : IRepositorySetRepository
  {
    private readonly IRepositoryRepository _repositoryRepository;

    public RepositorySetRepository(IRepositoryRepository repositoryRepository)
    {
      _repositoryRepository = repositoryRepository;
    }

    #region IRepositorySetRepository Members
    public RepositorySet FindDefaultRepositorySet()
    {
      return new RepositorySet(new Repository[] { _repositoryRepository.FindDefaultRepository() });
    }

    public void SaveRepositorySet(RepositorySet repository)
    {
    }
    #endregion
  }
}
