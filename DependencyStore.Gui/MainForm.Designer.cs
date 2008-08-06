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
      this._status = new System.Windows.Forms.ListView();
      this.SuspendLayout();
      // 
      // _bottom
      // 
      this._bottom.Dock = System.Windows.Forms.DockStyle.Bottom;
      this._bottom.Location = new System.Drawing.Point(0, 258);
      this._bottom.Name = "_bottom";
      this._bottom.Size = new System.Drawing.Size(417, 73);
      this._bottom.TabIndex = 1;
      // 
      // _status
      // 
      this._status.Dock = System.Windows.Forms.DockStyle.Fill;
      this._status.Location = new System.Drawing.Point(0, 0);
      this._status.Name = "_status";
      this._status.Size = new System.Drawing.Size(417, 258);
      this._status.TabIndex = 2;
      this._status.UseCompatibleStateImageBehavior = false;
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(417, 331);
      this.Controls.Add(this._status);
      this.Controls.Add(this._bottom);
      this.Name = "MainForm";
      this.Text = "DependencyStore";
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Panel _bottom;
    private System.Windows.Forms.ListView _status;
  }
}

