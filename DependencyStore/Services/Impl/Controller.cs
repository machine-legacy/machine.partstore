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
    private readonly ILocationRepository _locationRepository;
    private readonly IFileSystem _fileSystem;

    public Controller(ILocationRepository locationRepository, IFileAndDirectoryRulesRepository fileAndDirectoryRulesRepository, IProjectRepository projectRepository, IFileSystem fileSystem)
    {
      _projectRepository = projectRepository;
      _fileAndDirectoryRulesRepository = fileAndDirectoryRulesRepository;
      _locationRepository = locationRepository;
      _fileSystem = fileSystem;
    }

    #region IController Members
    public void Show(DependencyStoreConfiguration configuration)
    {
      DomainEvents.LocationNotFound += LocationNotFound;
      DomainEvents.Progress += Progress;
      SynchronizationPlan plan = CreatePlan(configuration);
      foreach (UpdateOutOfDateFile update in plan)
      {
        ReportOutdatedFile(update);
      }
    }

    public void Update(DependencyStoreConfiguration configuration)
    {
      DomainEvents.LocationNotFound += LocationNotFound;
      DomainEvents.Progress += Progress;
      SynchronizationPlan plan = CreatePlan(configuration);
      foreach (UpdateOutOfDateFile update in plan)
      {
        UpdateOutdatedFile(update);
      }
    }

    public void ArchiveProjects(DependencyStoreConfiguration configuration)
    {
      DomainEvents.Progress += Progress;
      BuildProjectArchives(configuration);
    }
    #endregion

    private SynchronizationPlan CreatePlan(DependencyStoreConfiguration configuration)
    {
      FileAndDirectoryRules rules = _fileAndDirectoryRulesRepository.FindDefault();
      IList<SourceLocation> sources = _locationRepository.FindAllSources(configuration, rules);
      IList<SinkLocation> sinks = _locationRepository.FindAllSinks(configuration, rules);
      LatestFileSet latestFiles = new LatestFileSet();
      latestFiles.AddAll(sources);
      SynchronizationPlan plan = new SynchronizationPlan();
      foreach (SinkLocation location in sinks)
      {
        plan.Merge(location.CreateSynchronizationPlan(latestFiles));
      }
      return plan;
    }

    private void BuildProjectArchives(DependencyStoreConfiguration configuration)
    {
      FileAndDirectoryRules rules = _fileAndDirectoryRulesRepository.FindDefault();
      IList<Project> projects = _projectRepository.FindAllProjects(configuration, rules);
      
      foreach (Project project in projects)
      {
        using (Archive archive = project.MakeArchive())
        {
          ZipArchiveWriter writer = new ZipArchiveWriter(archive);
          Purl path = configuration.PackageDirectory.Join(project.ArchiveName);
          writer.WriteZip(path);
        }
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
  }
}
