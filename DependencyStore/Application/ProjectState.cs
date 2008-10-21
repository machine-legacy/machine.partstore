using System;
using System.Collections.Generic;

using DependencyStore.Domain.Configuration;
using DependencyStore.Domain.Configuration.Repositories;
using DependencyStore.Domain.Core;
using DependencyStore.Domain.Core.Repositories;

namespace DependencyStore.Application
{
  public class ProjectState : IProjectState
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

    public bool Configure(string defaultRepositoryName)
    {
      DependencyStoreConfiguration configuration = _configurationRepository.FindProjectConfiguration();
      if (configuration == null)
      {
        configuration = new DependencyStoreConfiguration();
        configuration.Repositories.Add(new IncludeRepository(defaultRepositoryName));
      }
      _configurationRepository.SaveProjectConfiguration(configuration);
      return true;
    }
  }
}