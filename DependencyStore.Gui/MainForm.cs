using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace DependencyStore.Gui
{
  public partial class MainForm : Form, IStatusView
  {
    public MainForm()
    {
      InitializeComponent();
    }
  }
}
