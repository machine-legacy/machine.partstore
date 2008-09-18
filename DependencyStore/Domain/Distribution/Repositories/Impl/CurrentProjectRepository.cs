using System;
using System.Collections.Generic;

using DependencyStore.Domain.Configuration;
using DependencyStore.Domain.Configuration.Repositories;
using DependencyStore.Domain.Core;

namespace DependencyStore.Domain.Distribution.Repositories.Impl
{
  public class CurrentProjectRepository : ICurrentProjectRepository
  {
    private readonly ICurrentConfiguration _currentConfiguration;

    public CurrentProjectRepository(ICurrentConfiguration currentConfiguration)
    {
      _currentConfiguration = currentConfiguration;
    }

    #region ICurrentProjectRepository Members
    public CurrentProject FindCurrentProject()
    {
      ProjectConfiguration projectConfiguration = _currentConfiguration.DefaultConfiguration.ProjectConfigurations[0];
      Purl rootDirectory = projectConfiguration.Root.AsPurl;
      Purl buildDirectory = projectConfiguration.Build.AsPurl;
      Purl libraryDirectory = projectConfiguration.Library.AsPurl;
      string name = projectConfiguration.Name;
      return new CurrentProject(name, rootDirectory, buildDirectory, libraryDirectory);
    }

    public void SaveCurrentProject(CurrentProject project)
    {
      Console.WriteLine("THIS SHOULD WRITE TO DISK");
      foreach (ProjectReference reference in project.References)
      {
      }
    }
    #endregion
  }
}
