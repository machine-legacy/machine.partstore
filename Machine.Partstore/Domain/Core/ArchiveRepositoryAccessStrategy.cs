using System;
using Machine.Core.Services;

using Machine.Partstore.Domain.Archiving;
using Machine.Partstore.Domain.FileSystem;

namespace Machine.Partstore.Domain.Core
{
  public class ArchiveRepositoryAccessStrategy : IRepositoryAccessStrategy
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(ArchiveRepositoryAccessStrategy));

    private readonly IFileSystem _fileSystem;

    public ArchiveRepositoryAccessStrategy(IFileSystem fileSystem)
    {
      _fileSystem = fileSystem;
    }

    #region IRepositoryAdditionStrategy Members
    public void CommitVersionToRepository(Repository repository, NewProjectVersion newProjectVersion)
    {
      _log.Info("Committing: " + newProjectVersion);
      using (Archive archive = MakeArchiveFor(newProjectVersion))
      {
        ZipPackager writer = new ZipPackager(archive);
        writer.WriteZip(new Purl(newProjectVersion.PathInRepository.AsString + ZipPackager.ZipExtension));
      }
    }

    public void CheckoutVersionFromRepository(Repository repository, ArchivedProjectVersion version, Purl directory)
    {
      _log.Info("Checking out: " + version + " into " + directory);
      Archive archive = ArchiveFactory.ReadZip(new Purl(version.PathInRepository.AsString + ZipPackager.ZipExtension));
      ZipUnpackager unpackager = new ZipUnpackager(archive);
      unpackager.UnpackageZip(directory);
    }

    public bool IsVersionPresentInRepository(Repository repository, ArchivedProjectVersion version)
    {
      return _fileSystem.IsFile(version.PathInRepository.AsString + ZipPackager.ZipExtension);
    }
    #endregion

    private static Archive MakeArchiveFor(NewProjectVersion newProjectVersion)
    {
      Archive archive = new Archive();
      Purl commonRootDirectory = newProjectVersion.FileSet.FindCommonDirectory();
      foreach (FileSystemFile file in newProjectVersion.FileSet.Files)
      {
        archive.Add(file.Path.ChangeRoot(commonRootDirectory), file);
      }
      return archive;
    }
  }
}