using System;
using System.Collections.Generic;

using Microsoft.Build.BuildEngine;

namespace DependencyStore.Domain.Distribution
{
  public class ProjectManifestStore
  {
    private readonly Project _project;
    private readonly List<ProjectManifest> _manifests = new List<ProjectManifest>();

    public ProjectManifestStore(Project project)
    {
      _project = project;
    }
  }
}