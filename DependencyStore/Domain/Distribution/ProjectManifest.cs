using System;
using System.Collections.Generic;

using DependencyStore.Domain.Core;

namespace DependencyStore.Domain.Distribution
{
  public class ProjectManifest
  {
    public static readonly string Extension = "projref";

    private string _projectName;
    private DateTime _versionCreatedAt;

    public string ProjectName
    {
      get { return _projectName; }
      set { _projectName = value; }
    }

    public DateTime VersionCreatedAt
    {
      get { return _versionCreatedAt; }
      set { _versionCreatedAt = value; }
    }

    public string FileName
    {
      get { return this.ProjectName + "." + Extension; }
    }

    public ProjectManifest()
    {
    }

    public ProjectManifest(string name, DateTime versionCreatedAt)
    {
      _projectName = name;
      _versionCreatedAt = versionCreatedAt;
    }

    public bool IsOlderThan(ArchivedProjectVersion version)
    {
      return this.VersionCreatedAt < version.CreatedAt;
    }

    public bool IsSameVersionAs(ArchivedProjectVersion version)
    {
      return this.VersionCreatedAt == version.CreatedAt;
    }

    public bool IsAcceptableFileName(Purl path)
    {
      return this.ProjectName.Equals(path.NameWithoutExtension, StringComparison.InvariantCultureIgnoreCase);
    }

    public override string ToString()
    {
      return "ProjectManifest<" + this.ProjectName + ", " + this.VersionCreatedAt + ">";
    }
  }
}
