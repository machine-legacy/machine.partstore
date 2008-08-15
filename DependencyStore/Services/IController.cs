using System;
using System.Collections.Generic;

using DependencyStore.Domain;
using DependencyStore.Domain.Configuration;

namespace DependencyStore.Services
{
  public interface IController
  {
    void Show(DependencyStoreConfiguration configuration);
    void Update(DependencyStoreConfiguration configuration);
    void AddLatestToRepository(DependencyStoreConfiguration configuration, Repository repository);
    void Unpack(DependencyStoreConfiguration configuration, Repository repository);
  }
}
