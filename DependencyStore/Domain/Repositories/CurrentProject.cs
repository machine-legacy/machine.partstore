using System;
using System.Collections.Generic;

namespace DependencyStore.Domain.Repositories
{
  public class CurrentProject
  {
    private readonly Purl _rootDirectory;

    public Purl RootDirectory
    {
      get { return _rootDirectory; }
    }

    public CurrentProject(Purl rootDirectory)
    {
      _rootDirectory = rootDirectory;
    }

    public void AddDependency()
    {
    }
  }
}
