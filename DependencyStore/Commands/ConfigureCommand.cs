using System;
using System.Collections.Generic;

using DependencyStore.Domain.Configuration;
using DependencyStore.Domain.Configuration.Repositories;

namespace DependencyStore.Commands
{
  public class ConfigureCommand : Command
  {
    private readonly IConfigurationRepository _configurationRepository;

    public ConfigureCommand(IConfigurationRepository configurationRepository)
    {
      _configurationRepository = configurationRepository;
    }

    public override CommandStatus Run()
    {
      DependencyStoreConfiguration configuration = new DependencyStoreConfiguration();
      configuration.Repositories.Add(new IncludeRepository("Default"));
      _configurationRepository.SaveProjectConfiguration(configuration);
      return CommandStatus.Success;
    }
  }
}
