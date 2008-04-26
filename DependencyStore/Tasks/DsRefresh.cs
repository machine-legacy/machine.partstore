using System;
using System.Collections.Generic;

using DependencyStore.Services;

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
      if (_dryRun)
      {
        container.Resolve<IController>().Show();
      }
      else
      {
        container.Resolve<IController>().Update();
      }
      return true;
    }
  }
}
