using System;
using System.Collections.Generic;

using Machine.Container;
using Machine.Core.Services.Impl;

using DependencyStore.Services.DataAccess.Impl;
using DependencyStore.Services.DataAccess;
using DependencyStore.Services.Impl;
using DependencyStore.Services;

namespace DependencyStore
{
  public class DependencyStoreContainer : MachineContainer
  {
    public override void ReadyForServices()
    {
      base.ReadyForServices();
      Add<FileSystem>();
      Add<Clock>();
      Add<FileSystemEntryRepository>();
      Add<FileAndDirectoryRulesRepository>();
      Add<ConfigurationRepository>();
      Add<LocationRepository>();
      Add<ProjectRepository>();
      Add<Controller>();
    }
  }
}
