using System;
using System.Collections.Generic;

namespace DependencyStore.Domain.Future
{
  public class Project
  {
    private ProjectDirectory _projectDirectory;

    public ProjectDirectory ProjectDirectory
    {
      get { return _projectDirectory; }
      set { _projectDirectory = value; }
    }
  }
}
