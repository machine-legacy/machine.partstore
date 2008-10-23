using System;
using System.Collections.Generic;

using Machine.Partstore.Domain.Configuration;
using Machine.Partstore.Domain.Configuration.Repositories;

namespace Machine.Partstore.Domain.Core.Repositories.Impl
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
      ProjectDirectory rootDirectory = projectConfiguration.Root.ToProjectDirectory();
      ProjectDirectory buildDirectory = projectConfiguration.Build.ToProjectDirectory();
      ProjectDirectory libraryDirectory = projectConfiguration.Library.ToProjectDirectory();
      ProjectManifestStore manifests = ProjectManifestStore.Null;
      if (!libraryDirectory.IsMissing)
      {
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
          projectReference.UnpackageIfNecessary();
        }
      }
      Infrastructure.ProjectManifestRepository.SaveProjectManifestStore(project.Manifests);
    }
    #endregion
  }
  public static class Mapping
  {
    public static ProjectDirectory ToProjectDirectory(this DirectoryConfiguration directoryConfiguration)
    {
      if (directoryConfiguration == null)
      {
        return ProjectDirectory.Missing;
      }
      return new ProjectDirectory(directoryConfiguration.AsPurl);
    }
  }
}
