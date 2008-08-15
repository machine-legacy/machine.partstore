using System;
using System.Collections.Generic;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace DependencyStore.Tasks
{
  public abstract class DsTask : Task
  {
    private string _projectName;
    private bool _dryRun;

    [Required]
    public string ProjectName
    {
      get { return _projectName; }
      set { _projectName = value; }
    }

    [Required]
    public bool DryRun
    {
      get { return _dryRun; }
      set { _dryRun = value; }
    }

  }
}