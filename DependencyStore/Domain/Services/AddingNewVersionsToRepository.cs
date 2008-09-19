using System;
using System.Collections.Generic;

using DependencyStore.Domain.Core;
using DependencyStore.Domain.Distribution;

namespace DependencyStore.Domain.Services
{
  [Machine.Container.Model.Transient]
  public class AddingNewVersionsToRepository
  {
    public void AddNewVersion(Repository repository, Project project)
    {
      ArchivedProject archivedProject = repository.FindOrCreateProject(project);
      ArchivedProjectVersion version = ArchivedProjectVersion.Create(archivedProject, repository);
      NewProjectVersion newProjectVersion = new NewProjectVersion(archivedProject, version, CreateFileSet(project));
      Repository.AccessStrategy.CommitVersionToRepository(newProjectVersion);
      archivedProject.AddVersion(version);
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
