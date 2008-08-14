using System;
using System.Collections.Generic;

using DependencyStore.Domain;
using DependencyStore.Domain.Configuration;

namespace DependencyStore.Services.DataAccess.Impl
{
  public class ProjectRepository : IProjectRepository
  {
    private readonly IFileSystemEntryRepository _fileSystemEntryRepository;

    public ProjectRepository(IFileSystemEntryRepository fileSystemEntryRepository)
    {
      _fileSystemEntryRepository = fileSystemEntryRepository;
    }

    #region IProjectRepository Members
    public IList<Project> FindAllProjects(DependencyStoreConfiguration configuration)
    {
      List<Project> projects = new List<Project>();
      foreach (ProjectConfiguration projectConfiguration in configuration.ProjectConfigurations)
      {
        Purl path = new Purl(projectConfiguration.Build.Path);
        FileSystemEntry fileSystemEntry = _fileSystemEntryRepository.FindEntry(path, configuration.FileAndDirectoryRules);
        if (fileSystemEntry != null)
        {
          SourceLocation location = new SourceLocation(path, fileSystemEntry);
          Project project = new Project(projectConfiguration.Name, location, projectConfiguration.Library.AsPurl);
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
