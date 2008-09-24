using System;

namespace DependencyStore.Domain.Distribution
{
  public class RepositoryChange
  {
  }
  public class ProjectVersionCommitted : RepositoryChange
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

    public ProjectVersionCommitted(ArchivedProject project, ArchivedProjectVersion version)
    {
      _project = project;
      _version = version;
    }
  }
}