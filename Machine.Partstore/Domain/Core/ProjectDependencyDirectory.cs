using System;

using DependencyStore.Domain.FileSystem;

namespace DependencyStore.Domain.Core
{
  public class ProjectDependencyDirectory
  {
    private readonly Purl _path;
    private readonly ProjectManifestStore _manifests;
    private readonly ArchivedProject _dependency;

    public ProjectDependencyDirectory(Project project, ArchivedProject dependency)
    {
      _dependency = dependency;
      _path = project.DependencyPackageDirectoryFor(dependency);
      _manifests = Infrastructure.ProjectManifestRepository.FindProjectManifestStore(_path);
    }

    public bool IsAnythingInstalled
    {
      get { return _manifests.ManifestFor(_dependency) != null; }
    }

    public bool HasVersionOlderThan(ArchivedProjectVersion version)
    {
      ProjectManifest manifest = _manifests.ManifestFor(_dependency);
      if (manifest == null)
      {
        return true;
      }
      return manifest.IsOlderThan(version);
    }

    public void UpdateInstalledVersion(RepositorySet repositorySet, ArchivedProjectVersion version)
    {
      Repository.AccessStrategy.CheckoutVersionFromRepository(version, _path);
      _manifests.AddManifestFor(new ArchivedProjectAndVersion(_dependency, version));
      Infrastructure.ProjectManifestRepository.SaveProjectManifestStore(_manifests);
    }
  }
}