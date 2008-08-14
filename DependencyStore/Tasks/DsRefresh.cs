using System;
using System.Collections.Generic;

using Microsoft.Build.Framework;

using Machine.Container;

using DependencyStore.Services;
using DependencyStore.Services.DataAccess;

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
      MachineContainer container = new MachineContainer();
      container.Initialize();
      container.PrepareForServices();
      ContainerRegistrationHelper helper = new ContainerRegistrationHelper(container);
      helper.AddServiceCollectionsFrom(typeof(DependencyStoreServices).Assembly);
      container.Start();
      IConfigurationRepository configurationRepository = container.Resolve.Object<IConfigurationRepository>();
      IController controller = container.Resolve.Object<IController>();
      if (_dryRun)
      {
        controller.Show(configurationRepository.FindDefaultConfiguration());
      }
      else
      {
        controller.Update(configurationRepository.FindDefaultConfiguration());
      }
      return true;
    }
  }
}
