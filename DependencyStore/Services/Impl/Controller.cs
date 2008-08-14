using System;
using System.Collections.Generic;

using DependencyStore.Domain;
using DependencyStore.Domain.Archiving;
using DependencyStore.Domain.Configuration;
using DependencyStore.Domain.Services;
using DependencyStore.Services.DataAccess;

using Machine.Core.Utility;
using Machine.Core.Services;

namespace DependencyStore.Services.Impl
{
  public class Controller : IController
  {
    private readonly IProjectRepository _projectRepository;
    private readonly IFileSystem _fileSystem;
    private readonly DependencyState _state;

    public Controller(IProjectRepository projectRepository, IFileSystem fileSystem, DependencyState state)
    {
      _projectRepository = projectRepository;
      _state = state;
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
      IList<Project> projects = _projectRepository.FindAllProjects(configuration);
      AddProjectsToRepository adder = new AddProjectsToRepository(configuration);
      adder.AddProjects(projects, repository);
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
