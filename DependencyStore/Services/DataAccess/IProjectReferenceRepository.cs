using System;
using System.Collections.Generic;

using DependencyStore.Domain.Configuration;
using DependencyStore.Domain.Repositories;

namespace DependencyStore.Services.DataAccess
{
  interface IProjectReferenceRepository
  {
    IList<ProjectReference> FindAllProjectReferences(DependencyStoreConfiguration configuration);
  }
}
