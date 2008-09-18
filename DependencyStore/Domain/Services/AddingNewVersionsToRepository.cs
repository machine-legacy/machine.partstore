using System;
using System.Collections.Generic;

using DependencyStore.Domain.Core;
using DependencyStore.Domain.Repositories;

namespace DependencyStore.Domain.Services
{
  [Machine.Container.Model.Transient]
  public class AddingNewVersionsToRepository
  {
    public void AddProjects(IEnumerable<Project> projects, Repository repository)
    {
      foreach (Project project in projects)
      {
        ArchivedProject archivedProject = repository.FindOrCreateProject(project);
        ArchivedProjectVersion version = ArchivedProjectVersion.Create(archivedProject, repository);
        /*
        using (Archive archive = project.MakeArchive())
        {
          ZipPackager writer = new ZipPackager(archive);
          Purl path = version.ArchivePath;
          writer.WriteZip(path);
          archivedProject.AddVersion(version);
        }
        */
        throw new NotImplementedException();
      }
    }
  }
}
