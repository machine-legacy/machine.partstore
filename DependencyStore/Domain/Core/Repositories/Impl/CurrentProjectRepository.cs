using System;
using System.Collections.Generic;

using DependencyStore.Domain.Configuration;
using DependencyStore.Domain.Configuration.Repositories;
using DependencyStore.Domain.FileSystem;

namespace DependencyStore.Domain.Core.Repositories.Impl
{
  public class CurrentProjectRepository : ICurrentProjectRepository
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(CurrentProjectRepository));
    private readonly ICurrentConfiguration _currentConfiguration;
    private readonly IProjectManifestRepository _projectManifestRepository;

    public CurrentProjectRepository(ICurrentConfiguration currentConfiguration, IProjectManifestRepository projectManifestRepository)
    {
      _currentConfiguration = currentConfiguration;
      _projectManifestRepository = projectManifestRepository;
    }

    #region ICurrentProjectRepository Members
    public CurrentProject FindCurrentProject()
    {
      ProjectConfiguration projectConfiguration = _currentConfiguration.DefaultConfiguration.ProjectConfigurations[0];
      Purl rootDirectory = projectConfiguration.Root.AsPurl;
      Purl buildDirectory = null;
      if (projectConfiguration.Build != null)
      {
        buildDirectory = projectConfiguration.Build.AsPurl;
      }
      Purl libraryDirectory = projectConfiguration.Library.AsPurl;
      ProjectManifestStore manifests = _projectManifestRepository.FindProjectManifestStore(libraryDirectory);
      _log.Info("CurrentProject: " + projectConfiguration.Name + " in " + rootDirectory.AsString);
      return new CurrentProject(projectConfiguration.Name, rootDirectory, buildDirectory, libraryDirectory, manifests);
    }

    public void SaveCurrentProject(CurrentProject project, Repository repository)
    {
      foreach (ProjectReference projectReference in project.References)
      {
        if (projectReference.Status.IsOlderVersionInstalled)
        {
          projectReference.UnpackageIfNecessary(repository);
        }
      }
      Infrastructure.ProjectManifestRepository.SaveProjectManifestStore(project.Manifests);
    }
    #endregion
  }
}
