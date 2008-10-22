using System;
using System.Collections.Generic;

using Machine.Partstore.Domain.Configuration;
using Machine.Partstore.Domain.Configuration.Repositories;
using Machine.Partstore.Domain.Core;
using Machine.Partstore.Domain.Core.Repositories;

namespace Machine.Partstore.Application
{
  public class ProjectState : IAmForProjectState
  {
    private readonly IConfigurationRepository _configurationRepository;
    private readonly ICurrentProjectRepository _currentProjectRepository;

    public ProjectState(IConfigurationRepository configurationRepository, ICurrentProjectRepository currentProjectRepository)
    {
      _configurationRepository = configurationRepository;
      _currentProjectRepository = currentProjectRepository;
    }

    public CurrentProjectState GetCurrentProjectState()
    {
      if (_configurationRepository.FindProjectConfiguration() == null)
      {
        return new CurrentProjectState(true);
      }
      CurrentProject project = _currentProjectRepository.FindCurrentProject();
      IEnumerable<ReferenceStatus> references = project.ReferenceStatuses;
      return new CurrentProjectState(project.Name, references);
    }

    public ConfigureResponse Configure(string defaultRepositoryName)
    {
      DependencyStoreConfiguration configuration = _configurationRepository.FindProjectConfiguration();
      if (configuration == null)
      {
        configuration = new DependencyStoreConfiguration();
        configuration.Repositories.Add(new IncludeRepository(defaultRepositoryName));
      }
      _configurationRepository.SaveProjectConfiguration(configuration);
      return new ConfigureResponse(configuration.ConfigurationPath.AsString);
    }
  }
}