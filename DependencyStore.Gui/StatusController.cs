using System;
using System.Collections.Generic;

using DependencyStore.Domain;
using DependencyStore.Domain.Configuration;
using DependencyStore.Services.DataAccess;
using Machine.Core.Services;

namespace DependencyStore.Gui
{
  public class DependencyState
  {
    private readonly ILocationRepository _locationRepository;
    private readonly IConfigurationRepository _configurationRepository;
    private readonly IFileAndDirectoryRulesRepository _fileAndDirectoryRulesRepository;
    private readonly ConfigurationPaths _configurationPaths;
    private DependencyStoreConfiguration _configuration;
    private FileAndDirectoryRules _rules;
    private IList<SourceLocation> _sources;
    private IList<SinkLocation> _sinks;
    private LatestFileSet _latestFiles;

    public LatestFileSet LatestFiles
    {
      get { return _latestFiles; }
    }

    public IList<SinkLocation> Sinks
    {
      get { return _sinks; }
    }

    public IList<SourceLocation> Sources
    {
      get { return _sources; }
    }

    public FileSetGroupedByLocation LatestFilesGroupByLocation
    {
      get { return FileSetGroupedByLocation.GroupFileSetIntoLocations(_sources, _latestFiles); }
    }

    public SynchronizationPlan SynchronizationPlan
    {
      get
      {
        SynchronizationPlan plan = new SynchronizationPlan();
        foreach (SinkLocation location in _sinks)
        {
          plan.Merge(location.CreateSynchronizationPlan(this.LatestFiles));
        }
        return plan;
      }
    }

    public DependencyState(ILocationRepository locationRepository, IConfigurationRepository configurationRepository, IFileAndDirectoryRulesRepository fileAndDirectoryRulesRepository, ConfigurationPaths configurationPaths)
    {
      _locationRepository = locationRepository;
      _configurationPaths = configurationPaths;
      _fileAndDirectoryRulesRepository = fileAndDirectoryRulesRepository;
      _configurationRepository = configurationRepository;
    }

    public void Refresh()
    {
      string path = _configurationPaths.FindConfigurationPath();
      _configuration = _configurationRepository.FindConfiguration(path);
      _rules = _fileAndDirectoryRulesRepository.FindDefault();
      _sinks = _locationRepository.FindAllSinks(_configuration, _rules);
      _sources = _locationRepository.FindAllSources(_configuration, _rules);
      _latestFiles = new LatestFileSet();
      _latestFiles.AddAll(_sources);
      _latestFiles.SortByModifiedAt();
    }
  }
  public class StatusController
  {
    private readonly IStatusView _view;
    private readonly ConfigurationPaths _configurationPaths;
    private readonly DependencyState _state;
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
      _state = new DependencyState(_locationRepository, _configurationRepository, _fileAndDirectoryRulesRepository, _configurationPaths);
    }

    public void Start()
    {
      _view.Synchronize += OnSynchronize;
    }

    public void UpdateView()
    {
      _state.Refresh();
      _view.LatestFiles = _state.LatestFilesGroupByLocation;
      _view.SynchronizationPlan = _state.SynchronizationPlan;
    }

    private void OnSynchronize(object sender, EventArgs e)
    {
      _state.Refresh();
      foreach (UpdateOutOfDateFile update in _state.SynchronizationPlan)
      {
        _fileSystem.CopyFile(update.SourceFile.Purl.AsString, update.SinkFile.Purl.AsString, true);
      }
    }
  }
}
