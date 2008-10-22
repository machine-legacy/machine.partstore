using System;
using System.Collections.Generic;

using Machine.Container;
using Machine.Container.Plugins;
using Machine.Core.Services.Impl;

using Machine.Partstore.Domain.Configuration;
using Machine.Partstore.Commands;
using Machine.Partstore.Domain.FileSystem.Repositories.Impl;
using Machine.Partstore.Domain.Configuration.Repositories.Impl;
using Machine.Partstore.Domain.Core.Repositories.Impl;
using Machine.Partstore.Application;

namespace Machine.Partstore
{
  public class PartstoreServices : IServiceCollection
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
      register.Type<CurrentProjectRepository>();
      register.Type<RepositoryRepository>();
      register.Type<RepositorySetRepository>();

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
}
