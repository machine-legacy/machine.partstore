using System;
using System.Collections.Generic;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace DependencyStore.Tasks
{
  public abstract class DsTask : Task
  {
    private string _projectName;

    [Required]
    public string ProjectName
    {
      get { return _projectName; }
      set { _projectName = value; }
    }
  }
}