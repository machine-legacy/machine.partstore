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
      foreach (BuildDirectoryConfiguration directoryConfiguration in configuration.BuildDirectories)
      {
        FileSystemPath path = new FileSystemPath(directoryConfiguration.Path);
        FileSystemEntry fileSystemEntry = _fileSystemEntryRepository.FindEntry(path, rules);
        if (fileSystemEntry != null)
        {
          SourceLocation location = new SourceLocation(path, fileSystemEntry);
          Project project = new Project(directoryConfiguration.Name, location);
          projects.Add(project);
        }
        else
        {
          DomainEvents.OnLocationNotFound(this, new LocationNotFoundEventArgs(directoryConfiguration.AsFileSystemPath));
        }
      }
      return projects;
    }
    #endregion
  }
}
