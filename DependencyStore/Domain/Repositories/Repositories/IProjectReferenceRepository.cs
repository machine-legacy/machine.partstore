using System;
using System.Collections.Generic;

using DependencyStore.Domain.Core;

namespace DependencyStore.Domain.Repositories.Repositories
{
  public interface IProjectReferenceRepository
  {
    IList<ProjectReference> FindAllProjectReferences();
    ProjectReference FindProjectReferenceFor(Project parentProject, ArchivedProject dependency);
  }
}
