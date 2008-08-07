using System;
using System.Threading;
using Machine.Core.Services;

using DependencyStore.Domain;
using DependencyStore.Services.Impl;

namespace DependencyStore.Gui
{
  public class StatusController
  {
    private readonly IStatusView _view;
    private readonly DependencyState _state;
    private readonly IFileSystem _fileSystem;

    public StatusController(IStatusView view, IFileSystem fileSystem, DependencyState state)
    {
      _view = view;
      _state = state;
      _fileSystem = fileSystem;
    }

    public void Start()
    {
      _view.Synchronize += OnSynchronize;
      _view.Rescan += OnRescan;
    }

    public void UpdateView()
    {
      _state.Refresh();
      _view.LatestFiles = _state.LatestFilesGroupByLocation;
      _view.SynchronizationPlan = _state.SynchronizationPlan;
      _view.Log("Refreshed at {0}", DateTime.Now);
    }

    private void OnSynchronize(object sender, EventArgs e)
    {
      _state.Refresh();
      foreach (UpdateOutOfDateFile update in _state.SynchronizationPlan)
      {
        _view.Log("Copying {0} to {1}", update.SourceFile.Purl.AsString, update.SinkFile.Purl.AsString);
        _fileSystem.CopyFile(update.SourceFile.Purl.AsString, update.SinkFile.Purl.AsString, true);
      }
      _view.Log("Synchronized at {0}", DateTime.Now);
      UpdateView();
    }

    private void OnRescan(object sender, EventArgs e)
    {
      ThreadPool.QueueUserWorkItem((object ignored) => { UpdateView(); });
    }
  }
}
