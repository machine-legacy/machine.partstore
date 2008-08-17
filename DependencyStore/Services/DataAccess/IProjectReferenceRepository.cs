using System;
using System.Collections.Generic;

using DependencyStore.Domain.Repositories;

namespace DependencyStore.Services.DataAccess
{
  public interface IProjectReferenceRepository
  {
    IList<ProjectReference> FindAllProjectReferences();
  }
}
