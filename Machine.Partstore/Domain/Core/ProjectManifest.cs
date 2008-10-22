using System;
using System.Collections.Generic;

using Machine.Partstore.Domain.FileSystem;

namespace Machine.Partstore.Domain.Core
{
  public class ProjectManifest
  {
    public static readonly string Extension = "projref";

    private string _projectName;
    private VersionNumber _versionNumber;

    public string ProjectName
    {
      get { return _projectName; }
      set { _projectName = value; }
    }

    public VersionNumber VersionNumber
    {
      get { return _versionNumber; }
      set { _versionNumber = value; }
    }

    public string FileName
    {
      get { return _projectName + "." + Extension; }
    }

    public ProjectManifest()
    {
    }

    public ProjectManifest(string projectName, VersionNumber version)
    {
      _projectName = projectName;
      _versionNumber = version;
    }

    public bool IsOlderThan(ArchivedProjectVersion version)
    {
      return this.VersionNumber.IsOlderThan(version.Number);
    }

    public bool IsSameVersionAs(ArchivedProjectVersion version)
    {
      return this.VersionNumber.Equals(version.Number);
    }

    public bool IsAcceptableFileName(Purl path)
    {
      return this.FileName.Equals(path.Name, StringComparison.InvariantCultureIgnoreCase);
    }

    public override string ToString()
    {
      return "ProjectManifest<" + this.ProjectName + ", " + this.VersionNumber + ">";
    }
  }
}
