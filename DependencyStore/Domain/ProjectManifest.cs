using System;
using System.Collections.Generic;

namespace DependencyStore.Domain
{
  public class ProjectManifest
  {
    private string _name;
    private DateTime _versionCreatedAt;

    public string Name
    {
      get { return _name; }
      set { _name = value; }
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
      _name = name;
      _versionCreatedAt = versionCreatedAt;
    }

    public override string ToString()
    {
      return "ProjectManifest<" + this.Name + ", " + this.VersionCreatedAt + ">";
    }
  }
}
