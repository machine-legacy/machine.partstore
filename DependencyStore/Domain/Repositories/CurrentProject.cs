using System;
using System.Collections.Generic;

namespace DependencyStore.Domain.Repositories
{
  public class CurrentProject : Project
  {
    public CurrentProject(string name, Purl rootDirectory, Purl buildDirectory, Purl libraryDirectory)
      : base(name, rootDirectory, buildDirectory, libraryDirectory)
    {
    }
  }
}
