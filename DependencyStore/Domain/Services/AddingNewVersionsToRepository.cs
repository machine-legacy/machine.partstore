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
        using (Archive archive = MakeArchiveFor(project))
        {
          ZipPackager writer = new ZipPackager(archive);
          Purl path = version.ArchivePath;
          writer.WriteZip(path);
          archivedProject.AddVersion(version);
        }
      }
    }

    private static Archive MakeArchiveFor(Project project)
    {
      FileSystemEntry entry = Core.Infrastructure.FileSystemEntryRepository.FindEntry(project.BuildDirectory);
      FileSet fileSet = new FileSet();
      fileSet.AddAll(entry.BreadthFirstFiles);
      Purl fileRootDirectory = fileSet.FindCommonDirectory();
      Archive archive = new Archive();
      foreach (FileSystemFile file in fileSet.Files)
      {
        archive.Add(file.Path.ChangeRoot(fileRootDirectory), file);
      }
      return archive;
    }
  }
}
