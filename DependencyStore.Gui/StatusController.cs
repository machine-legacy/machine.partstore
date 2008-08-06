using System;
using System.Collections.Generic;

namespace DependencyStore.Gui
{
  public class StatusController
  {
    private readonly IStatusView _view;

    public StatusController(IStatusView view)
    {
      _view = view;
    }

    public void UpdateView()
    {
    }
  }
}
