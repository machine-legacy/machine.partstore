using System;
using System.Collections.Generic;

using Machine.Container;
using Machine.Container.Plugins;
using Machine.Core.Services.Impl;

using DependencyStore.Domain.Services;
using DependencyStore.Domain.Configuration;
using DependencyStore.Services.DataAccess.Impl;
using DependencyStore.Services.Impl;
using DependencyStore.Commands;

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
      register.Type<UnpackagingDependenciesForProjects>();
      register.Type<AddingDependenciesToProjects>();
      register.Type<CurrentConfiguration>();

      register.Type<CommandFactory>();
      register.Type<UnpackageCommand>();
      register.Type<AddNewVersionCommand>();
      register.Type<ShowCommand>();
      register.Type<AddNewVersionCommand>();
      register.Type<AddDependencyCommand>();
      register.Type<HelpCommand>();
    }
    #endregion
  }
}
