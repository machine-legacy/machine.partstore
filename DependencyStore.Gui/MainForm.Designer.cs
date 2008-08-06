namespace DependencyStore.Gui
{
  partial class MainForm
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this._bottom = new System.Windows.Forms.Panel();
      this._latestFilesList = new System.Windows.Forms.ListView();
      this._tabs = new System.Windows.Forms.TabControl();
      this._latestFilesTab = new System.Windows.Forms.TabPage();
      this._updatePlanTab = new System.Windows.Forms.TabPage();
      this._latestColumnFile = new System.Windows.Forms.ColumnHeader();
      this._latestColumnModifiedAt = new System.Windows.Forms.ColumnHeader();
      this._tabs.SuspendLayout();
      this._latestFilesTab.SuspendLayout();
      this.SuspendLayout();
      // 
      // _bottom
      // 
      this._bottom.Dock = System.Windows.Forms.DockStyle.Bottom;
      this._bottom.Location = new System.Drawing.Point(0, 321);
      this._bottom.Name = "_bottom";
      this._bottom.Size = new System.Drawing.Size(491, 73);
      this._bottom.TabIndex = 1;
      // 
      // _latestFilesList
      // 
      this._latestFilesList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this._latestColumnFile,
            this._latestColumnModifiedAt});
      this._latestFilesList.Dock = System.Windows.Forms.DockStyle.Fill;
      this._latestFilesList.Location = new System.Drawing.Point(3, 3);
      this._latestFilesList.Name = "_latestFilesList";
      this._latestFilesList.Size = new System.Drawing.Size(477, 289);
      this._latestFilesList.TabIndex = 2;
      this._latestFilesList.UseCompatibleStateImageBehavior = false;
      this._latestFilesList.View = System.Windows.Forms.View.Details;
      // 
      // _tabs
      // 
      this._tabs.Controls.Add(this._latestFilesTab);
      this._tabs.Controls.Add(this._updatePlanTab);
      this._tabs.Dock = System.Windows.Forms.DockStyle.Fill;
      this._tabs.Location = new System.Drawing.Point(0, 0);
      this._tabs.Name = "_tabs";
      this._tabs.SelectedIndex = 0;
      this._tabs.Size = new System.Drawing.Size(491, 321);
      this._tabs.TabIndex = 3;
      // 
      // _latestFilesTab
      // 
      this._latestFilesTab.Controls.Add(this._latestFilesList);
      this._latestFilesTab.Location = new System.Drawing.Point(4, 22);
      this._latestFilesTab.Name = "_latestFilesTab";
      this._latestFilesTab.Padding = new System.Windows.Forms.Padding(3);
      this._latestFilesTab.Size = new System.Drawing.Size(483, 295);
      this._latestFilesTab.TabIndex = 0;
      this._latestFilesTab.Text = "Latest Files";
      this._latestFilesTab.UseVisualStyleBackColor = true;
      // 
      // _updatePlanTab
      // 
      this._updatePlanTab.Location = new System.Drawing.Point(4, 22);
      this._updatePlanTab.Name = "_updatePlanTab";
      this._updatePlanTab.Padding = new System.Windows.Forms.Padding(3);
      this._updatePlanTab.Size = new System.Drawing.Size(483, 295);
      this._updatePlanTab.TabIndex = 1;
      this._updatePlanTab.Text = "Plan";
      this._updatePlanTab.UseVisualStyleBackColor = true;
      // 
      // _latestColumnFile
      // 
      this._latestColumnFile.Text = "File";
      this._latestColumnFile.Width = 317;
      // 
      // _latestColumnModifiedAt
      // 
      this._latestColumnModifiedAt.Text = "Last Modified";
      this._latestColumnModifiedAt.Width = 156;
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(491, 394);
      this.Controls.Add(this._tabs);
      this.Controls.Add(this._bottom);
      this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Name = "MainForm";
      this.Text = "DependencyStore";
      this._tabs.ResumeLayout(false);
      this._latestFilesTab.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Panel _bottom;
    private System.Windows.Forms.ListView _latestFilesList;
    private System.Windows.Forms.TabControl _tabs;
    private System.Windows.Forms.TabPage _latestFilesTab;
    private System.Windows.Forms.TabPage _updatePlanTab;
    private System.Windows.Forms.ColumnHeader _latestColumnFile;
    private System.Windows.Forms.ColumnHeader _latestColumnModifiedAt;
  }
}

