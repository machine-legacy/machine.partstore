using System;
using System.Collections.Generic;

using DependencyStore.Domain.Core;

namespace DependencyStore.Domain.Core.Repositories
{
  public interface IProjectRepository
  {
    IList<Project> FindAllProjects();
  }
}
