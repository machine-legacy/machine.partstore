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
    private readonly IRepositorySetRepository _repositorySetRepository;

    public CurrentProjectRepository(ICurrentConfiguration currentConfiguration, IProjectManifestRepository projectManifestRepository, IRepositorySetRepository repositorySetRepository)
    {
      _currentConfiguration = currentConfiguration;
      _repositorySetRepository = repositorySetRepository;
      _projectManifestRepository = projectManifestRepository;
    }

    #region ICurrentProjectRepository Members
    public CurrentProject FindCurrentProject()
    {
      RepositorySet repositorySet = _repositorySetRepository.FindDefaultRepositorySet();
      ProjectConfiguration projectConfiguration = _currentConfiguration.DefaultConfiguration.CurrentProjectConfiguration;
      ProjectDirectory rootDirectory = new ProjectDirectory(projectConfiguration.Root.AsPurl);
      ProjectDirectory buildDirectory = ProjectDirectory.Missing;
      if (projectConfiguration.Build != null)
      {
        buildDirectory = new ProjectDirectory(projectConfiguration.Build.AsPurl);
      }
      ProjectDirectory libraryDirectory = ProjectDirectory.Missing;
      ProjectManifestStore manifests = ProjectManifestStore.Null;
      if (projectConfiguration.Library != null)
      {
        libraryDirectory = new ProjectDirectory(projectConfiguration.Library.AsPurl);
        manifests = _projectManifestRepository.FindProjectManifestStore(libraryDirectory.Path);
      }
      _log.Info("CurrentProject: " + projectConfiguration.Name + " in " + rootDirectory.Path.AsString);
      return new CurrentProject(projectConfiguration.Name, rootDirectory, buildDirectory, libraryDirectory, repositorySet, manifests);
    }

    public void SaveCurrentProject(CurrentProject project)
    {
      foreach (ProjectReference projectReference in project.References)
      {
        if (projectReference.Status.IsOlderVersionInstalled)
        {
          projectReference.UnpackageIfNecessary(project.RepositorySet);
        }
      }
      Infrastructure.ProjectManifestRepository.SaveProjectManifestStore(project.Manifests);
    }
    #endregion
  }
}
