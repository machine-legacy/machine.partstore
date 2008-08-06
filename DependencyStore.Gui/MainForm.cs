using System;
using System.Collections.Generic;
using System.Windows.Forms;

using DependencyStore.Domain;

namespace DependencyStore.Gui
{
  public partial class MainForm : Form, IStatusView
  {
    private LatestFileSet _latestFiles;

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
        _latestFiles = value;
        AddLatestFilesToList();
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
  }
}
