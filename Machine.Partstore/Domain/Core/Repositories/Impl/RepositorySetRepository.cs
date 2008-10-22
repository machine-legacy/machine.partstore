using System;
using System.Collections.Generic;

using Machine.Partstore.Domain.Configuration;
using Machine.Partstore.Domain.Configuration.Repositories;

namespace Machine.Partstore.Domain.Core.Repositories.Impl
{
  public class RepositorySetRepository : IRepositorySetRepository
  {
    private readonly IRepositoryRepository _repositoryRepository;
    private readonly ICurrentConfiguration _currentConfiguration;

    public RepositorySetRepository(IRepositoryRepository repositoryRepository, ICurrentConfiguration currentConfiguration)
    {
      _repositoryRepository = repositoryRepository;
      _currentConfiguration = currentConfiguration;
    }

    #region IRepositorySetRepository Members
    public RepositorySet FindDefaultRepositorySet()
    {
      List<Repository> repositories = new List<Repository>();
      foreach (IncludeRepository includeRepository in _currentConfiguration.DefaultConfiguration.Repositories)
      {
        repositories.Add(_repositoryRepository.FindRepository(includeRepository.RepositoryDirectory));
      }
      return new RepositorySet(repositories);
    }

    public void SaveRepositorySet(RepositorySet repository)
    {
    }
    #endregion
  }
}
