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

    public void UpdateView()
    {
      DependencyStoreConfiguration configuration = GetConfiguration();
      FileAndDirectoryRules rules = _fileAndDirectoryRulesRepository.FindDefault();
      SynchronizationPlan plan = new SynchronizationPlan();
      IList<SinkLocation> sinks = _locationRepository.FindAllSinks(configuration, rules);
      IList<SourceLocation> sources = _locationRepository.FindAllSources(GetConfiguration(), rules);
      LatestFileSet latestFiles = new LatestFileSet();
      latestFiles.AddAll(sources);
      foreach (SinkLocation location in sinks)
      {
        plan.Merge(location.CreateSynchronizationPlan(latestFiles));
      }
      _view.LatestFiles = latestFiles;
      _view.SynchronizationPlan = plan;
    }

    private DependencyStoreConfiguration GetConfiguration()
    {
      string path = _configurationPaths.FindConfigurationPath();
      return _configurationRepository.FindConfiguration(path);
    }
  }
}
