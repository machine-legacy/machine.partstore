using System;
using System.Collections.Generic;

namespace DependencyStore.Domain.Core.Repositories
{
  public interface ICurrentProjectRepository
  {
    CurrentProject FindCurrentProject();
    void SaveCurrentProject(CurrentProject project, Repository repository);
  }
}
