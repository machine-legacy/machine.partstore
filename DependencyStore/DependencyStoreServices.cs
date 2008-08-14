using System;
using System.Collections.Generic;

using Machine.Container;
using Machine.Container.Plugins;
using Machine.Core.Services.Impl;

using DependencyStore.Domain.Configuration;
using DependencyStore.Services.DataAccess.Impl;
using DependencyStore.Services.Impl;

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
      register.Type<LocationRepository>();
      register.Type<ProjectRepository>();
      register.Type<Controller>();
      register.Type<DependencyState>();
      register.Type<ConfigurationPaths>();
      register.Type<RepositoryRepository>();
    }
    #endregion
  }
}
