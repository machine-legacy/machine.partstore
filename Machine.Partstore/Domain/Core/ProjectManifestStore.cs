using System;
using System.Collections.Generic;

using Machine.Partstore.Domain.FileSystem;

namespace Machine.Partstore.Domain.Core
{
  public class ProjectManifestStore : IEnumerable<ProjectManifest>
  {
    public static readonly ProjectManifestStore Null = new ProjectManifestStore(Purl.Null, new List<ProjectManifest>());
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

    public ProjectManifest ManifestFor(ArchivedProject project)
    {
      foreach (ProjectManifest manifest in _manifests)
      {
        if (manifest.ProjectName.Equals(project.Name, StringComparison.InvariantCultureIgnoreCase))
        {
          return manifest;
        }
      }
      return null;
    }

    public void AddManifestFor(ArchivedProjectAndVersion archivedProjectAndVersion)
    {
      RemoveManifestsForProject(archivedProjectAndVersion.Project);
      _manifests.Add(archivedProjectAndVersion.Project.MakeManifestFor(archivedProjectAndVersion));
    }

    private void RemoveManifestsForProject(ArchivedProject project)
    {
      ProjectManifest manifest = ManifestFor(project);
      if (manifest != null)
      {
        _manifests.Remove(manifest);
      }
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
  }
}