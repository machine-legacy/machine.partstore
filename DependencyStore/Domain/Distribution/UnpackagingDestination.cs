using System;

using DependencyStore.Domain.Archiving;
using DependencyStore.Domain.Core;

namespace DependencyStore.Domain.Distribution
{
  public class UnpackagingDestination
  {
    private readonly ProjectManifest _desiredVersionManifest;
    private readonly Purl _path;

    private Purl CurrentVersionManifestPath
    {
      get { return _path.Join(_desiredVersionManifest.FileName); }
    }

    public UnpackagingDestination(Project project, ProjectManifest desiredVersionManifest)
    {
      _path = project.LibraryDirectory.Join(desiredVersionManifest.ProjectName);
      _desiredVersionManifest = desiredVersionManifest;
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
      Infrastructure.ProjectManifestRepository.SaveProjectManifest(_desiredVersionManifest, this.CurrentVersionManifestPath);
    }
  }
}