using System;

using DependencyStore.Domain.Archiving;
using DependencyStore.Domain.Core;

namespace DependencyStore.Domain.Distribution
{
  public class ProjectDependencyDirectory
  {
    private readonly Purl _path;
    private readonly ProjectManifestStore _manifests;
    private readonly ArchivedProject _dependency;

    public ProjectDependencyDirectory(Project project, ArchivedProject dependency)
    {
      _dependency = dependency;
      _path = project.LibraryDirectory.Join(dependency.Name);
      _manifests = Infrastructure.ProjectManifestRepository.FindProjectManifestStore(_path);
    }

    public bool HasAnythingInstalled
    {
      get { return _manifests.ManifestFor(_dependency) != null; }
    }

    public bool HasVersionOlderThan(ArchivedProjectVersion version)
    {
      if (!this.HasAnythingInstalled)
      {
        return true;
      }
      ProjectManifest manifest = _manifests.ManifestFor(_dependency);
      if (manifest == null)
      {
        return true;
      }
      return manifest.IsOlderThan(version);
    }

    public void UpdateInstalledVersion(ArchivedProjectVersion version)
    {
      Archive archive = ArchiveFactory.ReadZip(version.ArchivePath);
      ZipUnpackager unpackager = new ZipUnpackager(archive);
      unpackager.UnpackageZip(_path);
      _manifests.AddManifestFor(_dependency, version);
      Infrastructure.ProjectManifestRepository.SaveProjectManifestStore(_manifests);
    }
  }
}