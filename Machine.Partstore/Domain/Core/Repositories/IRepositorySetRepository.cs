using System;
using System.Collections.Generic;

namespace Machine.Partstore.Domain.Core.Repositories
{
  public interface IRepositorySetRepository
  {
    RepositorySet FindDefaultRepositorySet();
    void SaveRepositorySet(RepositorySet repository);
  }
}