using System;

namespace DependencyStore.Domain.Core
{
  public class RepositoryChange
  {
  }
  public class ProjectVersionAdded : RepositoryChange
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

    public ProjectVersionAdded(ArchivedProject project, ArchivedProjectVersion version)
    {
      _project = project;
      _version = version;
    }
  }
}