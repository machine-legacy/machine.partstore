using System;

using DependencyStore.Domain.Archiving;
using DependencyStore.Domain.Core;

namespace DependencyStore.Domain.Distribution
{
  public class ArchiveRepositoryAccessStrategy : IRepositoryAccessStrategy
  {
    #region IRepositoryAdditionStrategy Members
    public void CommitVersionToRepository(NewProjectVersion newProjectVersion)
    {
      using (Archive archive = MakeArchiveFor(newProjectVersion))
      {
        ZipPackager writer = new ZipPackager(archive);
        writer.WriteZip(newProjectVersion.ArchivePath);
      }
    }

    public void CheckoutVersionFromRepository(ArchivedProjectVersion version, Purl directory)
    {
      Archive archive = ArchiveFactory.ReadZip(version.ArchivePath);
      ZipUnpackager unpackager = new ZipUnpackager(archive);
      unpackager.UnpackageZip(directory);
    }
    #endregion

    private static Archive MakeArchiveFor(NewProjectVersion newProjectVersion)
    {
      Archive archive = new Archive();
      foreach (FileSystemFile file in newProjectVersion.FileSet.Files)
      {
        archive.Add(file.Path.ChangeRoot(newProjectVersion.CommonRootDirectory), file);
      }
      return archive;
    }
  }
}