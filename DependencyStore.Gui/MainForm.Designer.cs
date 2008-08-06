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
      this._synchronizeButton = new System.Windows.Forms.Button();
      this._latestFilesView = new System.Windows.Forms.ListView();
      this._latestColumnFile = new System.Windows.Forms.ColumnHeader();
      this._latestColumnModifiedAt = new System.Windows.Forms.ColumnHeader();
      this._tabs = new System.Windows.Forms.TabControl();
      this._filesTab = new System.Windows.Forms.TabPage();
      this._synchronizationPlanTab = new System.Windows.Forms.TabPage();
      this._planView = new System.Windows.Forms.ListView();
      this._planColumn1 = new System.Windows.Forms.ColumnHeader();
      this._planColumn2 = new System.Windows.Forms.ColumnHeader();
      this._noPlan = new System.Windows.Forms.Label();
      this._bottom.SuspendLayout();
      this._tabs.SuspendLayout();
      this._filesTab.SuspendLayout();
      this._synchronizationPlanTab.SuspendLayout();
      this.SuspendLayout();
      // 
      // _bottom
      // 
      this._bottom.Controls.Add(this._synchronizeButton);
      this._bottom.Dock = System.Windows.Forms.DockStyle.Bottom;
      this._bottom.Location = new System.Drawing.Point(0, 390);
      this._bottom.Name = "_bottom";
      this._bottom.Size = new System.Drawing.Size(573, 34);
      this._bottom.TabIndex = 1;
      // 
      // _synchronizeButton
      // 
      this._synchronizeButton.Location = new System.Drawing.Point(7, 6);
      this._synchronizeButton.Name = "_synchronizeButton";
      this._synchronizeButton.Size = new System.Drawing.Size(99, 23);
      this._synchronizeButton.TabIndex = 0;
      this._synchronizeButton.Text = "Synchronize";
      this._synchronizeButton.UseVisualStyleBackColor = true;
      this._synchronizeButton.Click += new System.EventHandler(this.OnClickSynchronize);
      // 
      // _latestFilesView
      // 
      this._latestFilesView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this._latestColumnFile,
            this._latestColumnModifiedAt});
      this._latestFilesView.Dock = System.Windows.Forms.DockStyle.Fill;
      this._latestFilesView.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this._latestFilesView.FullRowSelect = true;
      this._latestFilesView.Location = new System.Drawing.Point(3, 3);
      this._latestFilesView.Name = "_latestFilesView";
      this._latestFilesView.Size = new System.Drawing.Size(559, 358);
      this._latestFilesView.TabIndex = 2;
      this._latestFilesView.UseCompatibleStateImageBehavior = false;
      this._latestFilesView.View = System.Windows.Forms.View.Details;
      // 
      // _latestColumnFile
      // 
      this._latestColumnFile.Text = "File";
      this._latestColumnFile.Width = 278;
      // 
      // _latestColumnModifiedAt
      // 
      this._latestColumnModifiedAt.Text = "Last Modified";
      this._latestColumnModifiedAt.Width = 156;
      // 
      // _tabs
      // 
      this._tabs.Controls.Add(this._synchronizationPlanTab);
      this._tabs.Controls.Add(this._filesTab);
      this._tabs.Dock = System.Windows.Forms.DockStyle.Fill;
      this._tabs.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this._tabs.Location = new System.Drawing.Point(0, 0);
      this._tabs.Name = "_tabs";
      this._tabs.SelectedIndex = 0;
      this._tabs.Size = new System.Drawing.Size(573, 390);
      this._tabs.TabIndex = 3;
      // 
      // _filesTab
      // 
      this._filesTab.Controls.Add(this._latestFilesView);
      this._filesTab.Location = new System.Drawing.Point(4, 22);
      this._filesTab.Name = "_filesTab";
      this._filesTab.Padding = new System.Windows.Forms.Padding(3);
      this._filesTab.Size = new System.Drawing.Size(565, 364);
      this._filesTab.TabIndex = 0;
      this._filesTab.Text = "Latest Files";
      this._filesTab.UseVisualStyleBackColor = true;
      // 
      // _synchronizationPlanTab
      // 
      this._synchronizationPlanTab.Controls.Add(this._planView);
      this._synchronizationPlanTab.Controls.Add(this._noPlan);
      this._synchronizationPlanTab.Location = new System.Drawing.Point(4, 22);
      this._synchronizationPlanTab.Name = "_synchronizationPlanTab";
      this._synchronizationPlanTab.Padding = new System.Windows.Forms.Padding(3);
      this._synchronizationPlanTab.Size = new System.Drawing.Size(565, 364);
      this._synchronizationPlanTab.TabIndex = 1;
      this._synchronizationPlanTab.Text = "Synchronization Plan";
      this._synchronizationPlanTab.UseVisualStyleBackColor = true;
      // 
      // _planView
      // 
      this._planView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this._planColumn1,
            this._planColumn2});
      this._planView.Dock = System.Windows.Forms.DockStyle.Fill;
      this._planView.Location = new System.Drawing.Point(3, 3);
      this._planView.Name = "_planView";
      this._planView.Size = new System.Drawing.Size(559, 358);
      this._planView.TabIndex = 0;
      this._planView.UseCompatibleStateImageBehavior = false;
      this._planView.View = System.Windows.Forms.View.Details;
      this._planView.Visible = false;
      // 
      // _planColumn1
      // 
      this._planColumn1.Text = "Out of Date";
      this._planColumn1.Width = 285;
      // 
      // _planColumn2
      // 
      this._planColumn2.Text = "Updated From";
      this._planColumn2.Width = 249;
      // 
      // _noPlan
      // 
      this._noPlan.Dock = System.Windows.Forms.DockStyle.Fill;
      this._noPlan.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this._noPlan.Location = new System.Drawing.Point(3, 3);
      this._noPlan.Name = "_noPlan";
      this._noPlan.Size = new System.Drawing.Size(559, 358);
      this._noPlan.TabIndex = 1;
      this._noPlan.Text = "Everything is up to date.";
      this._noPlan.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      this._noPlan.Visible = true;
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(573, 424);
      this.Controls.Add(this._tabs);
      this.Controls.Add(this._bottom);
      this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Name = "MainForm";
      this.Text = "DependencyStore";
      this._bottom.ResumeLayout(false);
      this._tabs.ResumeLayout(false);
      this._filesTab.ResumeLayout(false);
      this._synchronizationPlanTab.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Panel _bottom;
    private System.Windows.Forms.ListView _latestFilesView;
    private System.Windows.Forms.TabControl _tabs;
    private System.Windows.Forms.TabPage _filesTab;
    private System.Windows.Forms.TabPage _synchronizationPlanTab;
    private System.Windows.Forms.ColumnHeader _latestColumnFile;
    private System.Windows.Forms.ColumnHeader _latestColumnModifiedAt;
    private System.Windows.Forms.ListView _planView;
    private System.Windows.Forms.ColumnHeader _planColumn1;
    private System.Windows.Forms.ColumnHeader _planColumn2;
    private System.Windows.Forms.Button _synchronizeButton;
    private System.Windows.Forms.Label _noPlan;
  }
}

