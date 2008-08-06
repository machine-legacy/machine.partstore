using System;
using System.Collections.Generic;
using System.Windows.Forms;

using DependencyStore.Domain.Configuration;

namespace DependencyStore.Gui
{
  public static class Program
  {
    [STAThread]
    public static void Main()
    {
      using (DependencyStoreContainer container = new DependencyStoreContainer())
      {
        container.Initialize();
        container.PrepareForServices();
        container.Start();
        container.Add<ConfigurationPaths>();
        container.Add<MainForm>();

        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(container.Resolve.Object<MainForm>());
      }
    }
  }
}
