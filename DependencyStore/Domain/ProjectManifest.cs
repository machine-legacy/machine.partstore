using System;
using System.Collections.Generic;

namespace DependencyStore.Domain
{
  public class ProjectManifest
  {
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

    public override string ToString()
    {
      return "ProjectManifest<" + this.ProjectName + ", " + this.VersionCreatedAt + ">";
    }

    public bool IsAcceptableFileName(Purl path)
    {
      return this.ProjectName.Equals(path.Name, StringComparison.InvariantCultureIgnoreCase);
    }
  }
}
