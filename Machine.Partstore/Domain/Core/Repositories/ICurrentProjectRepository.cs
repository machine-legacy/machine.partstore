using System;
using System.Collections.Generic;

namespace Machine.Partstore.Domain.Core.Repositories
{
  public interface ICurrentProjectRepository
  {
    CurrentProject FindCurrentProject();
    void SaveCurrentProject(CurrentProject project);
  }
}
