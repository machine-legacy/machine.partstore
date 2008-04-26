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
    public override void Initialize()
    {
      base.Initialize();
      AddService<FileSystem>();
      AddService<Clock>();
      AddService<FileSystemEntryRepository>();
      AddService<FileAndDirectoryRulesRepository>();
      AddService<ConfigurationRepository>();
      AddService<LocationRepository>();
      AddService<Controller>();
    }
  }
}
