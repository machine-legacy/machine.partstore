using System;
using System.Collections.Generic;

using DependencyStore.Domain;
using DependencyStore.Domain.Configuration;

namespace DependencyStore.Services.DataAccess.Impl
{
  public class ProjectRepository : IProjectRepository
  {
    private readonly ICurrentConfiguration _currentConfiguration;

    public ProjectRepository(ICurrentConfiguration currentConfiguration)
    {
      _currentConfiguration = currentConfiguration;
    }

    #region IProjectRepository Members
    public IList<Project> FindAllProjects()
    {
      List<Project> projects = new List<Project>();
      foreach (ProjectConfiguration projectConfiguration in _currentConfiguration.DefaultConfiguration.ProjectConfigurations)
      {
        Purl buildDirectory = null;
        if (projectConfiguration.Build != null)
        {
          buildDirectory = projectConfiguration.Build.AsPurl;
        }
        Purl libraryDirectory = null;
        if (projectConfiguration.Library != null)
        {
          libraryDirectory = projectConfiguration.Library.AsPurl;
        }
        Project project = new Project(projectConfiguration.Name, buildDirectory, libraryDirectory);
        projects.Add(project);
      }
      return projects;
    }
    #endregion
  }
}
