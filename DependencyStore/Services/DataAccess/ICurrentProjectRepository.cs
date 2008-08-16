using System;
using System.Collections.Generic;

using DependencyStore.Domain.Repositories;

namespace DependencyStore.Services.DataAccess
{
  public interface ICurrentProjectRepository
  {
    CurrentProject FindCurrentProject();
  }
}
