using System;
using System.Collections.Generic;

using Machine.Partstore.Application;
using Machine.Partstore.Commands;
using Machine.Partstore.Domain.Configuration;
using Machine.Partstore.Domain.Configuration.Repositories;
using Machine.Partstore.Domain.Configuration.Repositories.Impl;
using Machine.Partstore.Domain.Core.Repositories;
using Machine.Partstore.Domain.Core.Repositories.Impl;
using Machine.Partstore.Domain.FileSystem;
using Machine.Partstore.Domain.FileSystem.Repositories;
using Machine.Partstore.Domain.FileSystem.Repositories.Impl;

using Machine.Container;
using Machine.Container.Plugins;
using Machine.Core.Services.Impl;
using Machine.Specifications;

using Rhino.Mocks;

namespace Machine.Partstore
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
      _projectManifestRepository = MockRepository.GenerateStub<IProjectManifestRepository>();
      _fileSystemEntryRepository = MockRepository.GenerateStub<IFileSystemEntryRepository>();
      _currentProjectRepository = MockRepository.GenerateStub<ICurrentProjectRepository>();
      _repositorySetRepository = MockRepository.GenerateStub<IRepositorySetRepository>();
      _repositoryRepository = MockRepository.GenerateStub<IRepositoryRepository>();
      _configurationRepository = MockRepository.GenerateStub<IConfigurationRepository>();
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
      services.ConfigurationRepository.Stub(x => x.FindProjectConfiguration()).Return(configuration);
      mocks.ReplayAll();

      projectState = container.Resolve.Object<ProjectState>();
    };

    Because of = () =>
      response = projectState.Configure("TestRepository");

    It should_return_configuration_file_path = () =>
      response.ConfigurationFile.ShouldNotBeNull();

    It should_save_the_configuration = () =>
      services.ConfigurationRepository.AssertWasCalled(x => x.SaveProjectConfiguration(configuration));
  }
}
