using System;
using System.Collections.Generic;
using System.Threading;
using Machine.Core.Services;

namespace DependencyStore.Gui
{
  public class ApplicationController : IDisposable, IRunnable
  {
    private readonly StatusController _statusController;
    private readonly IThreadManager _threadManager;
    private readonly object _lock = new object();
    private IThread _thread;
    private bool _running;

    public ApplicationController(IThreadManager threadManager, StatusController statusController)
    {
      _threadManager = threadManager;
      _statusController = statusController;
    }

    public IDisposable Start()
    {
      _running = true;
      _thread = _threadManager.CreateThread(this);
      _thread.Start();
      _statusController.Start();
      return this;
    }

    public void Refresh()
    {
      _statusController.UpdateView();
    }

    #region IDisposable Members
    public void Dispose()
    {
      lock (_lock)
      {
        Monitor.Pulse(_lock);
      }
      _running = false;
      _thread.Join();
    }
    #endregion

    #region IRunnable Members
    public void Run()
    {
      while (_running)
      {
        Refresh();
        if (_running)
        {
          lock (_lock)
          {
            Monitor.Wait(_lock, TimeSpan.FromSeconds(60.0));
          }
        }
      }
    }
    #endregion
  }
}
