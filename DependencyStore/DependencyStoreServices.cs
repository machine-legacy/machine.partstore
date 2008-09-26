using System;
using System.Collections.Generic;

using Machine.Container;
using Machine.Container.Plugins;
using Machine.Core.Services.Impl;

using DependencyStore.Domain.Configuration;
using DependencyStore.Commands;
using DependencyStore.Domain.FileSystem.Repositories.Impl;
using DependencyStore.Domain.Configuration.Repositories.Impl;
using DependencyStore.Domain.Core.Repositories.Impl;

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
      register.Type<ConfigurationPaths>();
      register.Type<CurrentConfiguration>();
      
      register.Type<ProjectManifestRepository>();
      register.Type<ProjectReferenceRepository>();
      register.Type<CurrentProjectRepository>();
      register.Type<ProjectRepository>();
      register.Type<RepositoryRepository>();
      register.Type<RepositorySetRepository>();

      register.Type<SeachRepositoryCommand>();
      register.Type<CommandFactory>();
      register.Type<UnpackageCommand>();
      register.Type<ShowCommand>();
      register.Type<AddNewVersionCommand>();
      register.Type<AddDependencyCommand>();
      register.Type<RefreshCommand>();
      register.Type<HelpCommand>();
    }
    #endregion
  }
}
