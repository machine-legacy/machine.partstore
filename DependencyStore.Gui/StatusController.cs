using System;
using System.Collections.Generic;

using DependencyStore.Domain;
using DependencyStore.Domain.Configuration;
using DependencyStore.Services.DataAccess;

namespace DependencyStore.Gui
{
  public class StatusController
  {
    private readonly IStatusView _view;
    private readonly ConfigurationPaths _configurationPaths;
    private readonly IConfigurationRepository _configurationRepository;
    private readonly IFileAndDirectoryRulesRepository _fileAndDirectoryRulesRepository;
    private readonly ILocationRepository _locationRepository;

    public StatusController(IStatusView view, ConfigurationPaths configurationPaths, IConfigurationRepository configurationRepository, IFileAndDirectoryRulesRepository fileAndDirectoryRulesRepository, ILocationRepository locationRepository)
    {
      _view = view;
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
      _view.LatestFiles = GetLatestFiles();
      _view.SynchronizationPlan = GetSynchronizationPlan(latestFiles);
    }

    private void OnSynchronize(object sender, EventArgs e)
    {
    }

    private LatestFileSet GetLatestFiles()
    {
      FileAndDirectoryRules rules = _fileAndDirectoryRulesRepository.FindDefault();
      IList<SourceLocation> sources = _locationRepository.FindAllSources(GetConfiguration(), rules);
      LatestFileSet latestFiles = new LatestFileSet();
      latestFiles.AddAll(sources);
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
