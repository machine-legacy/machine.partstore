using System;
using System.Collections.Generic;

using Machine.Container;
using Machine.Container.Plugins;
using Machine.Core.Services.Impl;

using DependencyStore.Domain.Services;
using DependencyStore.Domain.Configuration;
using DependencyStore.Commands;
using DependencyStore.Services.Impl;
using DependencyStore.Domain.Core.Repositories.Impl;
using DependencyStore.Domain.Configuration.Repositories.Impl;
using DependencyStore.Domain.Distribution.Repositories.Impl;
using DependencyStore.Domain.SimpleCopying.Repositories.Impl;

namespace DependencyStore
{
  public class DependencyStoreServices : IServiceCollection
  {
    #region IServiceCollection Members
    public void RegisterServices(ContainerRegisterer register)
    {
      register.Type<FileSystem>();
      register.Type<Clock>();
      register.Type<FileSystemEntryRepository>();
      register.Type<FileAndDirectoryRulesRepository>();
      register.Type<ConfigurationRepository>();
      register.Type<ProjectManifestRepository>();
      register.Type<ProjectReferenceRepository>();
      register.Type<CurrentProjectRepository>();
      register.Type<LocationRepository>();
      register.Type<ProjectRepository>();
      register.Type<DependencyState>();
      register.Type<ConfigurationPaths>();
      register.Type<RepositoryRepository>();
      register.Type<AddingNewVersionsToRepository>();
      register.Type<CurrentConfiguration>();

      register.Type<SeachRepositoryCommand>();
      register.Type<CommandFactory>();
      register.Type<UnpackageCommand>();
      register.Type<ShowCommand>();
      register.Type<PublishNewVersionCommand>();
      register.Type<AddDependencyCommand>();
      register.Type<HelpCommand>();
    }
    #endregion
  }
}
