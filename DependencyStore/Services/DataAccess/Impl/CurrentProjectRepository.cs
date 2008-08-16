using System;
using System.Collections.Generic;

using DependencyStore.Domain.Repositories;

namespace DependencyStore.Services.DataAccess.Impl
{
  public class CurrentProjectRepository : ICurrentProjectRepository
  {
    #region ICurrentProjectRepository Members
    public CurrentProject FindCurrentProject()
    {
      return new CurrentProject(null);
    }
    #endregion
  }
}
