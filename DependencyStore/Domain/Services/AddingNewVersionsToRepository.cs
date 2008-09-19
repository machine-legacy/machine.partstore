using System;
using System.Collections.Generic;

using DependencyStore.Domain.Archiving;
using DependencyStore.Domain.Core;
using DependencyStore.Domain.Distribution;

namespace DependencyStore.Domain.Services
{
  [Machine.Container.Model.Transient]
  public class AddingNewVersionsToRepository
  {
    private readonly IRepositoryAccessStrategy _repositoryAccessStrategy = new ArchiveRepositoryAccessStrategy();

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
        FileSystemEntry entry = Core.Infrastructure.FileSystemEntryRepository.FindEntry(project.BuildDirectory);
        FileSet fileSet = new FileSet();
        fileSet.AddAll(entry.BreadthFirstFiles);
        NewProjectVersion newProjectVersion = new NewProjectVersion(archivedProject, version, fileSet);
        _repositoryAccessStrategy.AddVersionToRepository(newProjectVersion);
        archivedProject.AddVersion(version);
      }
    }
  }
}
