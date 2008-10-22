using System;
using System.Collections.Generic;

namespace Machine.Partstore.Domain.Core
{
  public class ArchivedProjectAndVersion
  {
    private readonly ArchivedProject _project;
    private readonly ArchivedProjectVersion _version;

    public ArchivedProject Project
    {
      get { return _project; }
    }

    public ArchivedProjectVersion Version
    {
      get { return _version; }
    }

    public ArchivedProjectAndVersion(ArchivedProject project, ArchivedProjectVersion version)
    {
      _project = project;
      _version = version;
    }

    public override bool Equals(object obj)
    {
      ArchivedProjectAndVersion other = obj as ArchivedProjectAndVersion;
      if (other != null)
      {
        return other.Project.Equals(this.Project) && other.Version.Equals(this.Version);
      }
      return false;
    }

    public override Int32 GetHashCode()
    {
      return this.Project.GetHashCode() ^ this.Version.GetHashCode();
    }

    public override string ToString()
    {
      return "ArchivedProjectAndVersion<" + this.Project + ", " + this.Version + ">";
    }
  }
}
