using System;
using System.Collections.Generic;

using DependencyStore.Domain.Core;

namespace DependencyStore.Domain.Distribution.Repositories
{
  public interface IProjectReferenceRepository
  {
    IList<ProjectReference> FindAllProjectReferences();
    ProjectReference FindProjectReferenceFor(Project parentProject, ArchivedProject dependency);
  }
}
