using System;

using DependencyStore.Domain.Archiving;
using DependencyStore.Domain.Core;

namespace DependencyStore.Domain.Distribution
{
  public class UnpackagingDestination
  {
    private readonly Purl _path;
    private readonly ProjectManifest _currentManifest;
    private readonly Purl _currentManifestPath;

    private Purl CurrentVersionManifestPath
    {
      get { return _currentManifestPath; }
    }

    public UnpackagingDestination(Project project, ArchivedProject dependency)
    {
      ProjectManifest manifest = Infrastructure.ProjectManifestRepository.ReadProjectManifest(project.LibraryDirectory.Join(dependency.ManifestFileName));
      _path = project.LibraryDirectory.Join(dependency.Name);
      _currentManifest = manifest;
      _currentManifestPath = _path.Join(dependency.ManifestFileName);
    }

    public bool HasVersionOlderThan(ArchivedProjectVersion version)
    {
      if (!Infrastructure.FileSystem.IsFile(CurrentVersionManifestPath.AsString))
      {
        return true;
      }
      ProjectManifest currentManifest = Infrastructure.ProjectManifestRepository.ReadProjectManifest(CurrentVersionManifestPath);
      return currentManifest.IsOlderThan(version);
    }

    public void UpdateInstalledVersion(ArchivedProjectVersion version)
    {
      Archive archive = ArchiveFactory.ReadZip(version.ArchivePath);
      ZipUnpackager unpackager = new ZipUnpackager(archive);
      unpackager.UnpackageZip(_path);
      Infrastructure.ProjectManifestRepository.SaveProjectManifest(_currentManifest, this.CurrentVersionManifestPath);
    }
  }
}