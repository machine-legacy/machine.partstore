using System;

namespace DependencyStore.Domain.Core
{
  public class RepositoryChange
  {
  }
  public class ProjectVersionAdded : RepositoryChange
  {
    private readonly ArchivedProjectAndVersion _archivedProjectAndVersion;

    public ArchivedProject Project
    {
      get { return _archivedProjectAndVersion.Project; }
    }

    public ArchivedProjectVersion Version
    {
      get { return _archivedProjectAndVersion.Version; }
    }

    public ProjectVersionAdded(ArchivedProjectAndVersion archivedProjectAndVersion)
    {
      _archivedProjectAndVersion = archivedProjectAndVersion;
    }
  }
}