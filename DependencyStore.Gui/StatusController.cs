using System;
using System.Collections.Generic;
using System.Threading;
using Machine.Core.Services;

using DependencyStore.Domain.Core;
using DependencyStore.Services.Impl;

namespace DependencyStore.Gui
{
  public class StatusController
  {
    private readonly IStatusView _view;
    private readonly IFileSystem _fileSystem;
    private readonly DependencyState _state;

    public StatusController(IStatusView view, IFileSystem fileSystem, DependencyState state)
    {
      _view = view;
      _state = state;
      _fileSystem = fileSystem;
    }

    public void Start()
    {
      _view.SynchronizeAll += OnSynchronizeAll;
      _view.Synchronize += OnSynchronize;
      _view.Rescan += OnRescan;
    }

    public void UpdateView()
    {
      _state.Refresh();
      _view.LatestFiles = _state.LatestFilesGroupByLocation;
      _view.SynchronizationPlan = _state.CreatePlanForEverything();
      _view.Log("Refreshed at {0}", DateTime.Now);
    }

    private void OnSynchronizeAll(object sender, EventArgs e)
    {
      ThreadPool.QueueUserWorkItem((object ignored) => {
        _state.Refresh();
        CarryOutPlan(_state.CreatePlanForEverything());
        _view.Log("Synchronized at {0}", DateTime.Now);
        UpdateView();
      });
    }

    private void OnSynchronize(object sender, LocationEventArgs e)
    {
      ThreadPool.QueueUserWorkItem((object ignored) => {
        _state.Refresh();
        CarryOutPlan(_state.CreatePlanFor((SinkLocation)e.Location));
        _view.Log("Synchronized {0} at {1}", e.Location, DateTime.Now);
        UpdateView();
      });
    }

    private void OnRescan(object sender, EventArgs e)
    {
      ThreadPool.QueueUserWorkItem((object ignored) => { UpdateView(); });
    }

    private void CarryOutPlan(IEnumerable<SynchronizationOperation> plan)
    {
      foreach (UpdateOutOfDateFile update in plan)
      {
        _view.Log("Copying {0} to {1}", update.SourceFile.Purl.AsString, update.SinkFile.Purl.AsString);
        _fileSystem.CopyFile(update.SourceFile.Purl.AsString, update.SinkFile.Purl.AsString, true);
      }
    }
  }
}
