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
      LatestFileSet latestFiles = GetLatestFiles();
      _view.LatestFiles = latestFiles;
    }

    private LatestFileSet GetLatestFiles()
    {
      string path = _configurationPaths.FindConfigurationPath();
      DependencyStoreConfiguration configuration = _configurationRepository.FindConfiguration(path);
      FileAndDirectoryRules rules = _fileAndDirectoryRulesRepository.FindDefault();
      IList<SourceLocation> sources = _locationRepository.FindAllSources(configuration, rules);
      LatestFileSet latestFiles = new LatestFileSet();
      latestFiles.AddAll(sources);
      return latestFiles;
    }
  }
}
