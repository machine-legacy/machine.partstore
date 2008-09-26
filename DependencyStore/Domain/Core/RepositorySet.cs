using System;
using System.Collections.Generic;

namespace DependencyStore.Domain.Core
{
  public class RepositorySet
  {
    private readonly List<Repository> _repositories;

    public IEnumerable<Repository> Repositories
    {
      get { return _repositories; }
    }

    public RepositorySet(IEnumerable<Repository> repositories)
    {
      _repositories = new List<Repository>(repositories);
    }

    public ArchivedProject FindProject(string name)
    {
      foreach (Repository repository in _repositories)
      {
        ArchivedProject project = repository.FindProject(name);
        if (project != null)
        {
          return project;
        }
      }
      return null;
    }
  }
}
