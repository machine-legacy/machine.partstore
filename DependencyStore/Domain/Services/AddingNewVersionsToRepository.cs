using System;
using System.Collections.Generic;

using DependencyStore.Domain.Archiving;
using DependencyStore.Domain.Configuration;
using DependencyStore.Domain.Repositories;

namespace DependencyStore.Domain.Services
{
  [Machine.Container.Model.Transient]
  public class AddingNewVersionsToRepository
  {
    private readonly DependencyStoreConfiguration _configuration;

    public AddingNewVersionsToRepository(DependencyStoreConfiguration configuration)
    {
      _configuration = configuration;
    }

    public void AddProjects(IEnumerable<Project> projects, Repository repository)
    {
      foreach (Project project in projects)
      {
        ArchivedProject archivedProject = repository.FindOrCreateProject(project);
        ArchivedProjectVersion version = ArchivedProjectVersion.Create(archivedProject);
        using (Archive archive = project.MakeArchive())
        {
          ZipPackager writer = new ZipPackager(archive);
          Purl path = _configuration.RepositoryDirectory.Join(version.ArchiveFileName);
          writer.WriteZip(path);
          archivedProject.AddVersion(version);
        }
      }
    }
  }
}
