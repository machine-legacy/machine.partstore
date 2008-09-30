using System;
using System.Collections.Generic;

namespace DependencyStore.Domain.Core.Repositories
{
  public interface IProjectReferenceRepository
  {
    IList<ProjectReference> FindProjectReferences(Project project);
  }
}
