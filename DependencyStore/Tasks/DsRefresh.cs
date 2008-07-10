using System;
using System.Collections.Generic;

using DependencyStore.Services;
using DependencyStore.Services.DataAccess;
using Microsoft.Build.Framework;

namespace DependencyStore.Tasks
{
  public class DsRefresh : DsTask
  {
    private bool _dryRun;

    [Required]
    public bool DryRun
    {
      get { return _dryRun; }
      set { _dryRun = value; }
    }

    public override bool Execute()
    {
      DependencyStoreContainer container = new DependencyStoreContainer();
      container.Initialize();
      IConfigurationRepository configurationRepository = container.Resolve.Object<IConfigurationRepository>();
      IController controller = container.Resolve.Object<IController>();
      if (_dryRun)
      {
        controller.Show(configurationRepository.FindConfiguration("DependencyStore.config"));
      }
      else
      {
        controller.Update(configurationRepository.FindConfiguration("DependencyStore.config"));
      }
      return true;
    }
  }
}
