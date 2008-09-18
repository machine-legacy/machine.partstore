using System;
using System.Collections.Generic;

using DependencyStore.Domain.Core;

namespace DependencyStore.Services.DataAccess
{
  public interface IProjectRepository
  {
    IList<Project> FindAllProjects();
  }
}
