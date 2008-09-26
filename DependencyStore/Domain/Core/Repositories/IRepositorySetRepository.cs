using System;
using System.Collections.Generic;

namespace DependencyStore.Domain.Core.Repositories
{
  public interface IRepositorySetRepository
  {
    RepositorySet FindDefaultRepositorySet();
    void SaveRepositorySet(RepositorySet repository);
  }
}