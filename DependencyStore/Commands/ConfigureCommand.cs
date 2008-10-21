using System;
using System.Collections.Generic;

using DependencyStore.Domain.Configuration;
using DependencyStore.Domain.Configuration.Repositories;

namespace DependencyStore.Commands
{
  public class ConfigureCommand : Command
  {
    private readonly IConfigurationRepository _configurationRepository;
    private string _repositoryName = "Default";

    public string RepositoryName
    {
      get { return _repositoryName; }
      set { _repositoryName = value; }
    }

    public ConfigureCommand(IConfigurationRepository configurationRepository)
    {
      _configurationRepository = configurationRepository;
    }

    public override CommandStatus Run()
    {
      DependencyStoreConfiguration configuration = _configurationRepository.FindProjectConfiguration();
      if (configuration == null)
      {
        configuration = new DependencyStoreConfiguration();
        configuration.Repositories.Add(new IncludeRepository(_repositoryName));
      }
      _configurationRepository.SaveProjectConfiguration(configuration);
      Console.WriteLine("Saved configuration " + configuration.ConfigurationPath.AsString);
      return CommandStatus.Success;
    }
  }
}
