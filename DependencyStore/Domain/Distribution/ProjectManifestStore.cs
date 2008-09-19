using System;
using System.Collections.Generic;

using DependencyStore.Domain.Core;

namespace DependencyStore.Domain.Distribution
{
  public class ProjectManifestStore : IEnumerable<ProjectManifest>
  {
    private readonly Purl _path;
    private readonly IList<ProjectManifest> _manifests;

    public Purl RootDirectory
    {
      get { return _path; }
    }

    public ProjectManifestStore(Purl path, IList<ProjectManifest> manifests)
    {
      _path = path;
      _manifests = manifests;
    }

    #region IEnumerable<ProjectManifest> Members
    public IEnumerator<ProjectManifest> GetEnumerator()
    {
      return _manifests.GetEnumerator();
    }
    #endregion

    #region IEnumerable Members
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
    #endregion

    public ProjectManifest ManifestFor(ArchivedProject project)
    {
      foreach (ProjectManifest manifest in _manifests)
      {
        if (manifest.ProjectName == project.Name)
        {
          return manifest;
        }
      }
      return null;
    }

    public void AddManifestFor(ArchivedProject project, ArchivedProjectVersion version)
    {
      ProjectManifest manifest = project.MakeManifestForLatestVersion();
      _manifests.Add(manifest);
    }
  }
}