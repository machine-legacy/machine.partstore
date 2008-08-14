using System;
using System.Collections.Generic;

using DependencyStore.Domain;
using DependencyStore.Domain.Archiving;
using DependencyStore.Domain.Configuration;
using DependencyStore.Services.DataAccess;

using Machine.Core.Utility;
using Machine.Core.Services;

namespace DependencyStore.Services.Impl
{
  public class Controller : IController
  {
    private readonly IFileAndDirectoryRulesRepository _fileAndDirectoryRulesRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IFileSystem _fileSystem;
    private readonly DependencyState _state;

    public Controller(IFileAndDirectoryRulesRepository fileAndDirectoryRulesRepository, IProjectRepository projectRepository, IFileSystem fileSystem, DependencyState state)
    {
      _projectRepository = projectRepository;
      _state = state;
      _fileAndDirectoryRulesRepository = fileAndDirectoryRulesRepository;
      _fileSystem = fileSystem;
    }

    #region IController Members
    public void Show(DependencyStoreConfiguration configuration)
    {
      WireDomainEvents();
      _state.Refresh();
      foreach (UpdateOutOfDateFile update in _state.CreatePlanForEverything())
      {
        ReportOutdatedFile(update);
      }
    }

    public void Update(DependencyStoreConfiguration configuration)
    {
      WireDomainEvents();
      _state.Refresh();
      foreach (UpdateOutOfDateFile update in _state.CreatePlanForEverything())
      {
        UpdateOutdatedFile(update);
      }
    }

    public void ArchiveProjects(DependencyStoreConfiguration configuration, Repository repository)
    {
      DomainEvents.Progress += Progress;
      BuildProjectArchives(configuration, repository);
    }
    #endregion

    private void BuildProjectArchives(DependencyStoreConfiguration configuration, Repository repository)
    {
      FileAndDirectoryRules rules = _fileAndDirectoryRulesRepository.FindDefault();
      IList<Project> projects = _projectRepository.FindAllProjects(configuration, rules);
      
      foreach (Project project in projects)
      {
        ArchivedProject archivedProject = repository.FindOrCreateProject(project);
        ArchivedProjectVersion version = ArchivedProjectVersion.Create();
        Console.WriteLine(version.CreatedAtVersion);
        archivedProject.AddVersion(version);
        /*
        using (Archive archive = project.MakeArchive())
        {
          ZipArchiveWriter writer = new ZipArchiveWriter(archive);
          Purl path = configuration.RepositoryDirectory.Join(project.ArchiveName);
          writer.WriteZip(path);
        }
        */
      }
    }

    private static void ReportOutdatedFile(UpdateOutOfDateFile update)
    {
      TimeSpan age = update.SourceFile.ModifiedAt - update.SinkFile.ModifiedAt;
      Purl chrooted = update.SinkFile.Purl.ChangeRoot(update.SinkLocation.Path);
      Console.WriteLine("  {0} ({1} old)", chrooted.AsString, TimeSpanHelper.ToPrettyString(age));
    }

    private void UpdateOutdatedFile(UpdateOutOfDateFile update)
    {
      try
      {
        ReportOutdatedFile(update);
        _fileSystem.CopyFile(update.SourceFile.Purl.AsString, update.SinkFile.Purl.AsString, true);
      }
      catch (Exception error)
      {
        Console.WriteLine("Error copying {0}: {1}", update.SinkFile.Purl.AsString, error.Message);
      }
    }

    private static void LocationNotFound(object sender, LocationNotFoundEventArgs e)
    {
      Console.WriteLine("Missing Location: {0}", e.Path);
    }
    
    private static void Progress(object sender, ProgressEventArgs e)
    {
      Console.Write("Archiving: {0}%\r", e.PercentComplete * 100);
    }

    private static void WireDomainEvents()
    {
      DomainEvents.LocationNotFound += LocationNotFound;
      DomainEvents.Progress += Progress;
    }
  }
}
