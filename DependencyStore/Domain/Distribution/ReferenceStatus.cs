using System;

namespace DependencyStore.Domain.Distribution
{
  public class ReferenceStatus
  {
    private readonly string _dependencyName;
    private readonly DateTime _referencedVersionCreatedAt;
    private readonly bool _isProjectMissing;
    private readonly bool _isReferencedVersionMissing;
    private readonly bool _isToLatestVersion;
    private readonly bool _isAnyVersionInstalled;
    private readonly bool _isOlderVersionInstalled;
    private readonly bool _isReferencedVersionInstalled;

    public string DependencyName
    {
      get { return _dependencyName; }
    }

    public DateTime ReferencedVersionCreatedAt
    {
      get { return _referencedVersionCreatedAt; }
    }

    public bool IsProjectMissing
    {
      get { return _isProjectMissing; }
    }

    public bool IsReferencedVersionMissing
    {
      get { return _isReferencedVersionMissing; }
    }

    public bool IsToLatestVersion
    {
      get { return _isToLatestVersion; }
    }

    public bool IsAnyVersionInstalled
    {
      get { return _isAnyVersionInstalled; }
    }

    public bool IsOlderVersionInstalled
    {
      get { return _isOlderVersionInstalled; }
    }

    public bool IsReferencedVersionInstalled
    {
      get { return _isReferencedVersionInstalled; }
    }

    public bool IsOutdated
    {
      get { return !this.IsToLatestVersion; }
    }

    protected ReferenceStatus(string dependencyName, DateTime referencedVersionCreatedAt, bool isProjectMissing, bool isReferencedVersionMissing)
    {
      _dependencyName = dependencyName;
      _referencedVersionCreatedAt = referencedVersionCreatedAt;
      _isProjectMissing = isProjectMissing;
      _isReferencedVersionMissing = isReferencedVersionMissing;
    }

    protected ReferenceStatus(string dependencyName, DateTime referencedVersionCreatedAt, bool isToLatestVersion, bool isAnyVersionInstalled, bool isOlderVersionInstalled, bool isReferencedVersionInstalled)
    {
      _dependencyName = dependencyName;
      _referencedVersionCreatedAt = referencedVersionCreatedAt;
      _isToLatestVersion = isToLatestVersion;
      _isAnyVersionInstalled = isAnyVersionInstalled;
      _isOlderVersionInstalled = isOlderVersionInstalled;
      _isReferencedVersionInstalled = isReferencedVersionInstalled;
    }

    public static ReferenceStatus Create(ArchivedProject dependency, ArchivedProjectVersion version, ProjectDependencyDirectory dependencyDirectory)
    {
      bool isAnyVersionInstalled = dependencyDirectory.IsAnythingInstalled;
      bool isReferencedVersionInstalled = !dependencyDirectory.HasVersionOlderThan(version);
      bool isOlderVersionInstalled = dependencyDirectory.HasVersionOlderThan(version);
      bool isToLatestVersion = dependency.LatestVersion == version;
      return new ReferenceStatus(dependency.Name, version.CreatedAt, isToLatestVersion, isAnyVersionInstalled, isOlderVersionInstalled, isReferencedVersionInstalled);
    }

    public static ReferenceStatus CreateMissingProject(ProjectManifest manifest)
    {
      return new ReferenceStatus(manifest.ProjectName, manifest.VersionCreatedAt, true, true);
    }

    public static ReferenceStatus CreateMissingVersion(ProjectManifest manifest)
    {
      return new ReferenceStatus(manifest.ProjectName, manifest.VersionCreatedAt, false, true);
    }
  }
}