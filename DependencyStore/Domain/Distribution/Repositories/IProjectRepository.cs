using System;
using System.Collections.Generic;

namespace DependencyStore.Domain.Distribution.Repositories
{
  public interface IProjectRepository
  {
    IList<Project> FindAllProjects();
  }
}
