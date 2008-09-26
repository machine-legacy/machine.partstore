using System;
using System.Collections.Generic;

using DependencyStore.Domain.FileSystem;
using DependencyStore.Domain.Configuration;
using DependencyStore.Domain.Configuration.Repositories;

namespace DependencyStore.Domain.Distribution.Repositories.Impl
{
  public class ProjectRepository : IProjectRepository
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(ProjectRepository));
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
        List<Purl> directories = new List<Purl>();
        Purl buildDirectory = null;
        if (projectConfiguration.Build != null)
        {
          buildDirectory = projectConfiguration.Build.AsPurl;
          directories.Add(buildDirectory);
        }
        Purl libraryDirectory = null;
        if (projectConfiguration.Library != null)
        {
          libraryDirectory = projectConfiguration.Library.AsPurl;
          directories.Add(libraryDirectory);
        }
        Purl rootDirectory = Purl.FindCommonDirectory(directories.ToArray());
        Project project = new Project(projectConfiguration.Name, rootDirectory, buildDirectory, libraryDirectory);
        projects.Add(project);
      }
      return projects;
    }
    #endregion
  }
}
