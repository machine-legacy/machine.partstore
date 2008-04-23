using System;
using System.Collections.Generic;

using DependencyStore.Services;

namespace DependencyStore.Tasks
{
  public class DsShow : DsTask
  {
    public override bool Execute()
    {
      DependencyStoreContainer container = new DependencyStoreContainer();
      container.Initialize();
      container.Resolve<IController>().Show();
      return true;
    }
  }
}
