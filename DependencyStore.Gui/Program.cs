using System;
using System.Collections.Generic;
using System.Windows.Forms;

using Machine.Core.Services.Impl;

using DependencyStore.Domain.Configuration;

namespace DependencyStore.Gui
{
  public static class Program
  {
    [STAThread]
    public static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);

      using (DependencyStoreContainer container = new DependencyStoreContainer())
      {
        container.Initialize();
        container.PrepareForServices();
        container.Start();
        container.Add<ThreadManager>();
        container.Add<ConfigurationPaths>();
        container.Add<MainForm>();
        container.Add<ApplicationController>();
        container.Add<StatusController>();

        using (container.Resolve.Object<ApplicationController>().Start())
        {
          Application.Run(container.Resolve.Object<MainForm>());
        }
      }
    }
  }
}
