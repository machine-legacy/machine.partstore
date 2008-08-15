using DependencyStore.Domain.Archiving;
using DependencyStore.Domain.Configuration;

namespace DependencyStore.Domain.Repositories
{
  public class UnpackagingDestination
  {
    private readonly DependencyStoreConfiguration _configuration;
    private readonly ProjectManifest _desiredVersionManifest;
    private readonly Purl _path;

    private Purl CurrentVersionManifestPath
    {
      get { return _path.Join("CurrentVersion.projref"); }
    }

    public UnpackagingDestination(DependencyStoreConfiguration configuration, Project project, ProjectManifest desiredVersionManifest)
    {
      _configuration = configuration;
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
      Archive archive = ArchiveFactory.ReadZip(_configuration.RepositoryDirectory.Join(version.ArchiveFileName));
      ZipUnpackager unpackager = new ZipUnpackager(archive);
      unpackager.UnpackageZip(_path);
      ProjectManifest manifest = _desiredVersionManifest;
      Infrastructure.ProjectManifestRepository.SaveProjectManifest(manifest, this.CurrentVersionManifestPath);
    }
  }
}