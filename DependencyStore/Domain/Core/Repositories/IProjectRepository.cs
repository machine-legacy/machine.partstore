using System;
using System.Collections.Generic;

namespace DependencyStore.Domain.Core.Repositories
{
  public interface IProjectRepository
  {
    IList<Project> FindAllProjects();
  }
}
