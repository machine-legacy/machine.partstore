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
    public IList<Project> FindAllProjects(DependencyStoreConfiguration configuration, FileAndDirectoryRules rules)
    {
      List<Project> projects = new List<Project>();
      foreach (ProjectConfiguration projectConfiguration in configuration.ProjectConfigurations)
      {
        FileSystemPath path = new FileSystemPath(projectConfiguration.Build.Path);
        FileSystemEntry fileSystemEntry = _fileSystemEntryRepository.FindEntry(path, rules);
        if (fileSystemEntry != null)
        {
          SourceLocation location = new SourceLocation(path, fileSystemEntry);
          Project project = new Project(projectConfiguration.Name, location);
          projects.Add(project);
        }
        else
        {
          DomainEvents.OnLocationNotFound(this, new LocationNotFoundEventArgs(projectConfiguration.Build.AsFileSystemPath));
        }
      }
      return projects;
    }
    #endregion
  }
}
