using System;
using System.Collections.Generic;

namespace DependencyStore.Domain.Distribution.Repositories
{
  public interface ICurrentProjectRepository
  {
    CurrentProject FindCurrentProject();
    void SaveCurrentProject(CurrentProject project);
  }
}
