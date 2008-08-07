using System;
using System.Collections.Generic;

using DependencyStore.Domain;
using DependencyStore.Domain.Configuration;
using DependencyStore.Services.DataAccess;
using Machine.Core.Services;

namespace DependencyStore.Gui
{
  public class StatusController
  {
    private readonly IStatusView _view;
    private readonly ConfigurationPaths _configurationPaths;
    private readonly IConfigurationRepository _configurationRepository;
    private readonly IFileAndDirectoryRulesRepository _fileAndDirectoryRulesRepository;
    private readonly ILocationRepository _locationRepository;
    private readonly IFileSystem _fileSystem;

    public StatusController(IStatusView view, ConfigurationPaths configurationPaths, IConfigurationRepository configurationRepository, IFileAndDirectoryRulesRepository fileAndDirectoryRulesRepository, ILocationRepository locationRepository, IFileSystem fileSystem)
    {
      _view = view;
      _fileSystem = fileSystem;
      _locationRepository = locationRepository;
      _fileAndDirectoryRulesRepository = fileAndDirectoryRulesRepository;
      _configurationRepository = configurationRepository;
      _configurationPaths = configurationPaths;
    }

    public void Start()
    {
      _view.Synchronize += OnSynchronize;
    }

    public void UpdateView()
    {
      LatestFileSet latestFiles = GetLatestFiles();
      FileAndDirectoryRules rules = _fileAndDirectoryRulesRepository.FindDefault();
      IList<SourceLocation> sources = _locationRepository.FindAllSources(GetConfiguration(), rules);
      FileSetGroupedByLocation groupedByLocation = FileSetGroupedByLocation.GroupFileSetIntoLocations(sources, latestFiles);
      _view.LatestFiles = groupedByLocation;
      _view.SynchronizationPlan = GetSynchronizationPlan(latestFiles);
    }

    private void OnSynchronize(object sender, EventArgs e)
    {
      LatestFileSet latestFiles = GetLatestFiles();
      SynchronizationPlan plan = GetSynchronizationPlan(latestFiles);
      foreach (UpdateOutOfDateFile update in plan)
      {
        _fileSystem.CopyFile(update.SourceFile.Purl.AsString, update.SinkFile.Purl.AsString, true);
      }
    }

    private LatestFileSet GetLatestFiles()
    {
      FileAndDirectoryRules rules = _fileAndDirectoryRulesRepository.FindDefault();
      IList<SourceLocation> sources = _locationRepository.FindAllSources(GetConfiguration(), rules);
      LatestFileSet latestFiles = new LatestFileSet();
      latestFiles.AddAll(sources);
      latestFiles.SortByModifiedAt();
      return latestFiles;
    }

    private SynchronizationPlan GetSynchronizationPlan(LatestFileSet latestFiles)
    {
      FileAndDirectoryRules rules = _fileAndDirectoryRulesRepository.FindDefault();
      SynchronizationPlan plan = new SynchronizationPlan();
      IList<SinkLocation> sinks = _locationRepository.FindAllSinks(GetConfiguration(), rules);
      foreach (SinkLocation location in sinks)
      {
        plan.Merge(location.CreateSynchronizationPlan(latestFiles));
      }
      return plan;
    }

    private DependencyStoreConfiguration GetConfiguration()
    {
      string path = _configurationPaths.FindConfigurationPath();
      return _configurationRepository.FindConfiguration(path);
    }
  }
}
