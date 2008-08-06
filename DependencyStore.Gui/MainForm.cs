using System;
using System.Collections.Generic;
using System.Windows.Forms;

using DependencyStore.Domain;

namespace DependencyStore.Gui
{
  public partial class MainForm : Form, IStatusView
  {
    private LatestFileSet _latestFiles;
    private SynchronizationPlan _synchronizationPlan;

    public MainForm()
    {
      InitializeComponent();
    }

    #region IStatusView Members
    public LatestFileSet LatestFiles
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
    #endregion

    private void AddLatestFilesToView()
    {
      _latestFilesView.Items.Clear();
      foreach (FileAsset file in _latestFiles.Files)
      {
        ListViewItem item = new ListViewItem(file.Purl.AsString);
        item.SubItems.Add(new ListViewItem.ListViewSubItem(item, file.ModifiedAt.ToString()));
        _latestFilesView.Items.Add(item);
      }
      _latestFilesView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
    }

    private bool IsDifferentEnoughToRedisplay(LatestFileSet newest)
    {
      if (_latestFiles == null)
      {
        return true;
      }
      FileSetComparer fileSetComparer = new FileSetComparer();
      FileSet changes = fileSetComparer.Compare(_latestFiles, newest);
      return !changes.IsEmpty;
    }

    private void AddSynchronizationPlanToView()
    {
      _planView.Items.Clear();
      foreach (UpdateOutOfDateFile update in _synchronizationPlan)
      {
        ListViewItem item = new ListViewItem(update.SinkFile.Purl.AsString);
        item.SubItems.Add(new ListViewItem.ListViewSubItem(item, update.SourceFile.Purl.AsString));
        _planView.Items.Add(item);
      }
      _planView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
    }

    private bool IsDifferentEnoughToRedisplay(SynchronizationPlan newestPlan)
    {
      if (_synchronizationPlan == null)
      {
        return true;
      }
      return true;
    }
  }
}
