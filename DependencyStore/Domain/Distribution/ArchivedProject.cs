using System;
using System.Collections.Generic;

namespace DependencyStore.Domain.Distribution
{
  public class ArchivedProject
  {
    private readonly List<ArchivedProjectVersion> _versions = new List<ArchivedProjectVersion>();
    private string _name;

    public string Name
    {
      get { return _name; }
      set { _name = value; }
    }

    public string ManifestFileName
    {
      get { return this.Name + "." + ProjectManifest.Extension; }
    }

    public List<ArchivedProjectVersion> Versions
    {
      get { return _versions; }
    }

    public ArchivedProjectVersion LatestVersion
    {
      get
      {
        _versions.Sort((x, y) => x.CreatedAtVersion.CompareTo(y.CreatedAtVersion));
        return _versions[_versions.Count - 1];
      }
    }

    public ArchivedProject()
    {
    }

    public ArchivedProject(string name)
    {
      _name = name;
    }

    public ArchivedProjectVersion FindVersionInManifest(ProjectManifest manifest)
    {
      return FindVersionByCreatedAt(manifest.VersionCreatedAt);
    }

    public ArchivedProjectVersion FindVersionByCreatedAt(DateTime createdAt)
    {
      foreach (ArchivedProjectVersion existing in _versions)
      {
        if (existing.CreatedAt.Equals(createdAt))
        {
          return existing;
        }
      }
      return null;
    }

    public void AddVersion(ArchivedProjectVersion version)
    {
      if (FindVersionByCreatedAt(version.CreatedAt) != null)
      {
        throw new InvalidOperationException("Duplicate project versions: " + this.Name + "-" + version.CreatedAt);
      }
      _versions.Add(version);
    }

    private ProjectManifest MakeManifest(ArchivedProjectVersion version)
    {
      return new ProjectManifest(this.Name, version.CreatedAt);
    }

    public ProjectManifest MakeManifestForLatestVersion()
    {
      return MakeManifest(this.LatestVersion);
    }

    public override string ToString()
    {
      return "ArchivedProject<" + this.Name + ">";
    }
  }
}