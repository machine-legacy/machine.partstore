using System;
using System.Collections.Generic;

using DependencyStore.Domain.Core;
using DependencyStore.Domain.Distribution;

namespace DependencyStore.Domain.Services
{
  [Machine.Container.Model.Transient]
  public class AddingNewVersionsToRepository
  {
    public void PublishProject(Project project, Repository repository)
    {
      AddProjects(new Project[] { project }, repository);
    }

    public void AddProjects(IEnumerable<Project> projects, Repository repository)
    {
      foreach (Project project in projects)
      {
        ArchivedProject archivedProject = repository.FindOrCreateProject(project);
        ArchivedProjectVersion version = ArchivedProjectVersion.Create(archivedProject, repository);
        NewProjectVersion newProjectVersion = new NewProjectVersion(archivedProject, version, CreateFileSet(project));
        Repository.AccessStrategy.CommitVersionToRepository(newProjectVersion);
        archivedProject.AddVersion(version);
      }
    }

    private static FileSet CreateFileSet(Project project)
    {
      FileSystemEntry entry = Core.Infrastructure.FileSystemEntryRepository.FindEntry(project.BuildDirectory);
      FileSet fileSet = new FileSet();
      fileSet.AddAll(entry.BreadthFirstFiles);
      return fileSet;
    }
  }
}
