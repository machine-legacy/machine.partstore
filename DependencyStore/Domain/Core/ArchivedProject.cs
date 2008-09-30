using System;
using System.Collections.Generic;

namespace DependencyStore.Domain.Core
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
        _versions.Sort((x, y) => x.Number.TimeStamp.CompareTo(y.Number.TimeStamp));
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
      return FindVersionByCreatedAt(manifest.VersionNumber);
    }

    public ArchivedProjectVersion FindVersionByCreatedAt(VersionNumber number)
    {
      foreach (ArchivedProjectVersion existing in _versions)
      {
        if (existing.Number.Equals(number))
        {
          return existing;
        }
      }
      return null;
    }

    public void AddVersion(ArchivedProjectVersion version)
    {
      if (FindVersionByCreatedAt(version.Number) != null)
      {
        throw new InvalidOperationException("Duplicate project versions: " + this.Name + "-" + version.Number.TimeStamp);
      }
      _versions.Add(version);
    }

    private ProjectManifest MakeManifest(ArchivedProjectVersion version)
    {
      return new ProjectManifest(this.Name, version.Number);
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