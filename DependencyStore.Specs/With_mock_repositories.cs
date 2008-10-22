using System;
using System.Collections.Generic;

using DependencyStore.Application;
using DependencyStore.Commands;
using DependencyStore.Domain.Configuration;
using DependencyStore.Domain.Configuration.Repositories;
using DependencyStore.Domain.Configuration.Repositories.Impl;
using DependencyStore.Domain.Core.Repositories;
using DependencyStore.Domain.Core.Repositories.Impl;
using DependencyStore.Domain.FileSystem;
using DependencyStore.Domain.FileSystem.Repositories;
using DependencyStore.Domain.FileSystem.Repositories.Impl;

using Machine.Container;
using Machine.Container.Plugins;
using Machine.Core.Services.Impl;
using Machine.Specifications;

using Rhino.Mocks;

namespace DependencyStore
{
  public class MockRepositoriesServices : IServiceCollection
  {
    private readonly MockRepository _mocks;
    private readonly IProjectManifestRepository _projectManifestRepository;
    private readonly IFileSystemEntryRepository _fileSystemEntryRepository;
    private readonly ICurrentProjectRepository _currentProjectRepository;
    private readonly IRepositoryRepository _repositoryRepository;
    private readonly IRepositorySetRepository _repositorySetRepository;
    private readonly IConfigurationRepository _configurationRepository;

    public MockRepository Mocks
    {
      get { return _mocks; }
    }

    public IProjectManifestRepository ProjectManifestRepository
    {
      get { return _projectManifestRepository; }
    }

    public IFileSystemEntryRepository FileSystemEntryRepository
    {
      get { return _fileSystemEntryRepository; }
    }

    public ICurrentProjectRepository CurrentProjectRepository
    {
      get { return _currentProjectRepository; }
    }

    public IRepositoryRepository RepositoryRepository
    {
      get { return _repositoryRepository; }
    }

    public IRepositorySetRepository RepositorySetRepository
    {
      get { return _repositorySetRepository; }
    }

    public IConfigurationRepository ConfigurationRepository
    {
      get { return _configurationRepository; }
    }

    public MockRepositoriesServices(MockRepository mocks)
    {
      _mocks = mocks;
      _projectManifestRepository = _mocks.DynamicMock<IProjectManifestRepository>();
      _fileSystemEntryRepository = _mocks.DynamicMock<IFileSystemEntryRepository>();
      _currentProjectRepository = _mocks.DynamicMock<ICurrentProjectRepository>();
      _repositorySetRepository = _mocks.DynamicMock<IRepositorySetRepository>();
      _repositoryRepository = _mocks.DynamicMock<IRepositoryRepository>();
      _configurationRepository = _mocks.DynamicMock<IConfigurationRepository>();
    }

    #region IServiceCollection Members
    public void RegisterServices(ContainerRegisterer register)
    {
      register.Type<FileSystem>();
      register.Type<Clock>();

      register.Type<IFileSystemEntryRepository>().Is(_fileSystemEntryRepository);
      register.Type<FileAndDirectoryRulesRepository>();
      
      register.Type<IConfigurationRepository>().Is(_configurationRepository);
      register.Type<ConfigurationPaths>();
      register.Type<CurrentConfiguration>();
      
      register.Type<IProjectManifestRepository>().Is(_projectManifestRepository);
      register.Type<ICurrentProjectRepository>().Is(_currentProjectRepository);
      register.Type<IRepositoryRepository>().Is(_repositoryRepository);
      register.Type<IRepositorySetRepository>().Is(_repositorySetRepository);

      register.Type<SeachRepositoryCommand>();
      register.Type<CommandFactory>();
      register.Type<UnpackageCommand>();
      register.Type<ShowCommand>();
      register.Type<AddNewVersionCommand>();
      register.Type<AddDependencyCommand>();
      register.Type<RefreshCommand>();
      register.Type<ConfigureCommand>();
      register.Type<HelpCommand>();

      register.Type<ProjectState>();
      register.Type<RepositorySets>();
      register.Type<ProjectDependencies>();
    }
    #endregion
  }
  public class with_mock_repositories
  {
    protected static MachineContainer container;
    protected static MockRepository mocks;
    protected static MockRepositoriesServices services;
    protected static NewCreators New = new NewCreators();

    Establish context = () =>
    {
      mocks = new MockRepository();
      services = new MockRepositoriesServices(mocks);
      container = new MachineContainer();
      container.Initialize();
      container.PrepareForServices();
      ContainerRegistrationHelper helper = new ContainerRegistrationHelper(container);
      helper.AddServiceCollection(services);
      container.Start();
      IoC.Container = container;
    };
  }

  public class with_configuration : with_mock_repositories
  {
    protected static DependencyStoreConfiguration configuration;

    Establish context = () =>
    {
      configuration = New.Configuration();
    };
  }
  
  public class when_configuring_with_configuration : with_configuration
  {
    static ConfigureResponse response;
    static ProjectState projectState;

    Establish context = () =>
    {
      SetupResult.For(services.ConfigurationRepository.FindProjectConfiguration()).Return(configuration);
      services.ConfigurationRepository.SaveProjectConfiguration(configuration);
      mocks.ReplayAll();

      projectState = container.Resolve.Object<ProjectState>();
    };

    Because of = () =>
      response = projectState.Configure("TestRepository");

    It should_return_configuration_file_path = () =>
      response.ConfigurationFile.ShouldNotBeNull();
  }
}
