using System;

namespace DependencyStore.Domain.Distribution
{
  public class ReferenceStatus
  {
    private readonly bool _isToLatestVersion;
    private readonly bool _isAnyVersionInstalled;
    private readonly bool _isOlderVersionInstalled;
    private readonly bool _isReferencedVersionInstalled;

    public bool IsProjectMissing
    {
      get { return false; }
    }

    public bool IsReferencedVersionMissing
    {
      get { return false; }
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

    public ReferenceStatus(bool isToLatestVersion, bool isAnyVersionInstalled, bool isOlderVersionInstalled, bool isReferencedVersionInstalled)
    {
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
      return new ReferenceStatus(isToLatestVersion, isAnyVersionInstalled, isOlderVersionInstalled, isReferencedVersionInstalled);
    }
  }
}