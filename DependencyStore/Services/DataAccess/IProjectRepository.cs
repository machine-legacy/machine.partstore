using System;
using System.Collections.Generic;

using DependencyStore.Domain;
using DependencyStore.Domain.Configuration;

namespace DependencyStore.Services.DataAccess
{
  public interface IProjectRepository
  {
    IList<Project> FindAllProjects(DependencyStoreConfiguration configuration);
  }
}
