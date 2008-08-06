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
          AddLatestFilesToList();
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
        _synchronizationPlan = value;
      }
    }
    #endregion

    private void AddLatestFilesToList()
    {
      _latestFilesList.Items.Clear();
      foreach (FileAsset file in _latestFiles.Files)
      {
        ListViewItem item = new ListViewItem(file.Purl.AsString);
        item.SubItems.Add(new ListViewItem.ListViewSubItem(item, file.ModifiedAt.ToString()));
        _latestFilesList.Items.Add(item);
      }
      _latestFilesList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
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
  }
}
