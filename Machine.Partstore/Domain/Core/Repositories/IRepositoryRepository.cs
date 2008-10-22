using System;
using System.Collections.Generic;

using Machine.Partstore.Domain.FileSystem;

namespace Machine.Partstore.Domain.Core.Repositories
{
  public interface IRepositoryRepository
  {
    Repository FindRepository(Purl path);
    void SaveRepository(Repository repository);
    void RefreshRepository(Repository repository);
  }
}
