using System;
using System.Collections.Generic;
using System.Windows.Forms;

using DependencyStore.Domain.Core;
using DependencyStore.Domain.SimpleCopying;

namespace DependencyStore.Gui
{
  public partial class MainForm : Form, IStatusView
  {
    private FileSetGroupedByLocation _latestFiles;
    private SynchronizationPlan _synchronizationPlan;

    public MainForm()
    {
      InitializeComponent();
    }

    #region IStatusView Members
    public event EventHandler<EventArgs> SynchronizeAll;
    public event EventHandler<LocationEventArgs> Synchronize;
    public event EventHandler<EventArgs> Rescan;

    public FileSetGroupedByLocation LatestFiles
    {
      get { return _latestFiles; }
      set
      {
        if (this.InvokeRequired)
        {
          Invoke(new MethodInvoker(delegate() { this.LatestFiles = value; }));
          return;
        }
        if (IsDifferentEnoughToRedisplay(value))
        {
          _latestFiles = value;
          AddLatestFilesToView();
        }
      }
    }

    public SynchronizationPlan SynchronizationPlan
    {
      get { return _synchronizationPlan; }
      set
      {
        if (this.InvokeRequired)
        {
          Invoke(new MethodInvoker(delegate() { this.SynchronizationPlan = value; }));
          return;
        }
        if (IsDifferentEnoughToRedisplay(value))
        {
          _synchronizationPlan = value;
          AddSynchronizationPlanToView();
        }
      }
    }

    public void Log(string message, params object[] args)
    {
      if (this.InvokeRequired)
      {
        Invoke(new MethodInvoker(delegate() { Log(message, args); }));
        return;
      }
      if (_log.IsDisposed) return;
      _log.Text += String.Format(message, args) + Environment.NewLine;
      _log.SelectionStart = _log.TextLength;
      _log.ScrollToCaret();
    }
    #endregion

    private void AddLatestFilesToView()
    {
      _latestFilesView.Items.Clear();
      _latestFilesView.Items.Clear();
      foreach (KeyValuePair<Location, FileSet> locationAndFiles in _latestFiles.LocationsAndFiles)
      {
        ListViewGroup group = new ListViewGroup(locationAndFiles.Key.Path.AsString);
        _latestFilesView.Groups.Add(group);
        foreach (FileAsset file in locationAndFiles.Value.Files)
        {
          ListViewItem item = new ListViewItem(file.Purl.ChangeRoot(locationAndFiles.Key.Path).AsString);
          item.SubItems.Add(new ListViewItem.ListViewSubItem(item, file.ModifiedAt.ToString()));
          item.Group = group;
          _latestFilesView.Items.Add(item);
        }
      }
    }

    private bool IsDifferentEnoughToRedisplay(FileSetGroupedByLocation newest)
    {
      if (_latestFiles == null)
      {
        return true;
      }
      FileSetComparer fileSetComparer = new FileSetComparer();
      FileSet changes = fileSetComparer.Compare(_latestFiles.AllFiles, newest.AllFiles);
      return !changes.IsEmpty;
    }

    private void AddSynchronizationPlanToView()
    {
      if (_synchronizationPlan.IsEmpty)
      {
        _noPlan.Visible = true;
        _planView.Visible = false;
        return;
      }
      _planView.Items.Clear();
      _planView.Groups.Clear();
      Dictionary<SinkLocation, ListViewGroup> groups = new Dictionary<SinkLocation, ListViewGroup>();
      foreach (UpdateOutOfDateFile update in _synchronizationPlan)
      {
        if (!groups.ContainsKey(update.SinkLocation))
        {
          groups[update.SinkLocation] = new ListViewGroup(update.SinkLocation.Path.AsString);
          groups[update.SinkLocation].Tag = update.SinkLocation;
          _planView.Groups.Add(groups[update.SinkLocation]);
        }
        ListViewItem item = new ListViewItem(update.SinkFile.Purl.ChangeRoot(update.SinkLocation.Path).AsString);
        item.SubItems.Add(new ListViewItem.ListViewSubItem(item, update.SourceFile.Purl.AsString));
        item.Group = groups[update.SinkLocation];
        _planView.Items.Add(item);
      }
      _noPlan.Visible = false;
      _planView.Visible = true;
    }

    private bool IsDifferentEnoughToRedisplay(SynchronizationPlan newestPlan)
    {
      if (_synchronizationPlan == null)
      {
        return true;
      }
      return true;
    }

    private void OnClickSynchronize(object sender, EventArgs e)
    {
      if (this.SynchronizeAll == null) return;
      this.SynchronizeAll(sender, e);
    }

    private void OnClickRefresh(object sender, EventArgs e)
    {
      if (this.Rescan == null) return;
      this.Rescan(sender, e);
    }

    private void OnClickClose(object sender, EventArgs e)
    {
      Close();
    }

    private void OnDoubleClickPlan(object sender, EventArgs e)
    {
      if (this.Synchronize == null) return;
      if (_planView.SelectedItems.Count == 0)
      {
        return;
      }
      foreach (ListViewItem item in _planView.SelectedItems)
      {
        SinkLocation location = (SinkLocation)item.Group.Tag;
        this.Synchronize(sender, new LocationEventArgs(location));
      }
    }
  }
}
