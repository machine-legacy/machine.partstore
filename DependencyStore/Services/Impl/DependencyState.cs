using System;
using System.Collections.Generic;

using DependencyStore.Domain;
using DependencyStore.Domain.Configuration;
using DependencyStore.Services.DataAccess;

namespace DependencyStore.Services.Impl
{
  public class DependencyState
  {
    private readonly ILocationRepository _locationRepository;
    private readonly IConfigurationRepository _configurationRepository;
    private readonly IFileAndDirectoryRulesRepository _fileAndDirectoryRulesRepository;
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

    public DependencyState(ILocationRepository locationRepository, IConfigurationRepository configurationRepository, IFileAndDirectoryRulesRepository fileAndDirectoryRulesRepository)
    {
      _locationRepository = locationRepository;
      _fileAndDirectoryRulesRepository = fileAndDirectoryRulesRepository;
      _configurationRepository = configurationRepository;
    }

    public void Refresh()
    {
      _configuration = _configurationRepository.FindDefaultConfiguration();
      _rules = _fileAndDirectoryRulesRepository.FindDefault();
      _sinks = _locationRepository.FindAllSinks(_configuration, _rules);
      _sources = _locationRepository.FindAllSources(_configuration, _rules);
      _latestFiles = new LatestFileSet();
      _latestFiles.AddAll(_sources);
      _latestFiles.SortByModifiedAt();
    }

    public SynchronizationPlan CreatePlanForEverything()
    {
      SynchronizationPlan plan = new SynchronizationPlan();
      foreach (SinkLocation location in _sinks)
      {
        plan.Merge(location.CreateSynchronizationPlan(this.LatestFiles));
      }
      return plan;
    }

    public SynchronizationPlan CreatePlanFor(SinkLocation location)
    {
      return location.CreateSynchronizationPlan(this.LatestFiles);
    }
  }
}