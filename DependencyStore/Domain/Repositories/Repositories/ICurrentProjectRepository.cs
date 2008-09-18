using System;
using System.Collections.Generic;

namespace DependencyStore.Domain.Repositories.Repositories
{
  public interface ICurrentProjectRepository
  {
    CurrentProject FindCurrentProject();
  }
}
