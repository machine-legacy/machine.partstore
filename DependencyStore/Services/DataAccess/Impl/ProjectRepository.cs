using System;
using System.Collections.Generic;

using DependencyStore.Domain;
using DependencyStore.Domain.Configuration;

namespace DependencyStore.Services.DataAccess.Impl
{
  public class ProjectRepository : IProjectRepository
  {
    private readonly ICurrentConfiguration _currentConfiguration;
    private readonly IFileSystemEntryRepository _fileSystemEntryRepository;

    public ProjectRepository(ICurrentConfiguration currentConfiguration, IFileSystemEntryRepository fileSystemEntryRepository)
    {
      _currentConfiguration = currentConfiguration;
      _fileSystemEntryRepository = fileSystemEntryRepository;
    }

    #region IProjectRepository Members
    public IList<Project> FindAllProjects()
    {
      List<Project> projects = new List<Project>();
      foreach (ProjectConfiguration projectConfiguration in _currentConfiguration.DefaultConfiguration.ProjectConfigurations)
      {
        Purl path = new Purl(projectConfiguration.Build.Path);
        FileSystemEntry fileSystemEntry = _fileSystemEntryRepository.FindEntry(path, _currentConfiguration.DefaultConfiguration.FileAndDirectoryRules);
        if (fileSystemEntry != null)
        {
          SourceLocation location = new SourceLocation(path, fileSystemEntry);
          Purl libraryDirectory = null;
          if (projectConfiguration.Library != null)
          {
            libraryDirectory = projectConfiguration.Library.AsPurl;
          }
          Project project = new Project(projectConfiguration.Name, location, libraryDirectory);
          projects.Add(project);
        }
        else
        {
          DomainEvents.OnLocationNotFound(this, new LocationNotFoundEventArgs(projectConfiguration.Build.AsPurl));
        }
      }
      return projects;
    }
    #endregion
  }
}
