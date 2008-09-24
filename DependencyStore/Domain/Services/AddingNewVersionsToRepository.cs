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
      version.FileSet = CreateFileSet(project.BuildDirectory);
      archivedProject.AddVersion(version);
    }

    private static FileSet CreateFileSet(Purl directory)
    {
      FileSystemEntry entry = Core.Infrastructure.FileSystemEntryRepository.FindEntry(directory);
      FileSet fileSet = new FileSet();
      fileSet.AddAll(entry.BreadthFirstFiles);
      return fileSet;
    }

    public void CommitNewVersion(ProjectVersionCommitted change)
    {
      NewProjectVersion newProjectVersion = new NewProjectVersion(change.Project, change.Version, change.Version.FileSet);
      Repository.AccessStrategy.CommitVersionToRepository(newProjectVersion);
    }
  }
}
