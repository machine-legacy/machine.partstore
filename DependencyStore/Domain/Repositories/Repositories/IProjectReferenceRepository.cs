using System;
using System.Collections.Generic;

namespace DependencyStore.Domain.Repositories.Repositories
{
  public interface IProjectReferenceRepository
  {
    IList<ProjectReference> FindAllProjectReferences();
  }
}
