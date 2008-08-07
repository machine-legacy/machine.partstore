using System;
using System.Collections.Generic;
using System.Windows.Forms;

using DependencyStore.Domain;

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

    public event EventHandler<EventArgs> Synchronize;
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
      _latestFilesView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
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
          _planView.Groups.Add(groups[update.SinkLocation]);
        }
        ListViewItem item = new ListViewItem(update.SinkFile.Purl.ChangeRoot(update.SinkLocation.Path).AsString);
        item.SubItems.Add(new ListViewItem.ListViewSubItem(item, update.SourceFile.Purl.AsString));
        item.Group = groups[update.SinkLocation];
        _planView.Items.Add(item);
      }
      _planView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
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
      if (this.Synchronize == null) return;
      this.Synchronize(sender, e);
    }
  }
}
