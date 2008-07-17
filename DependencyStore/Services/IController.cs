using System;
using System.Collections.Generic;

using DependencyStore.Domain.Configuration;

namespace DependencyStore.Services
{
  public interface IController
  {
    void Show(DependencyStoreConfiguration configuration);
    void Update(DependencyStoreConfiguration configuration);
    void ArchiveProjects(DependencyStoreConfiguration configuration);
  }
}
