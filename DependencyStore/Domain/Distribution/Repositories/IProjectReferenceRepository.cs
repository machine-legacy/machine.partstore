using System;
using System.Collections.Generic;

namespace DependencyStore.Domain.Distribution.Repositories
{
  public interface IProjectReferenceRepository
  {
    IList<ProjectReference> FindAllProjectReferences();
  }
}
