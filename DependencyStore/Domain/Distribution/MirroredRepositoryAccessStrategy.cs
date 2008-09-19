using System;
using System.Collections.Generic;

using DependencyStore.Domain.Archiving;
using DependencyStore.Domain.Core;

namespace DependencyStore.Domain.Distribution
{
  public class MirroredRepositoryAccessStrategy : IRepositoryAccessStrategy
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(MirroredRepositoryAccessStrategy));

    #region IRepositoryAccessStrategy Members
    public void CommitVersionToRepository(NewProjectVersion newProjectVersion)
    {
      _log.Info("Committing: " + newProjectVersion);
      Purl destiny = newProjectVersion.PathInRepository;
      CopyFiles(newProjectVersion.FileSet, destiny);
    }

    public void CheckoutVersionFromRepository(ArchivedProjectVersion version, Purl directory)
    {
      _log.Info("Checking out: " + version + " into " + directory);
      FileSystemEntry entry = Infrastructure.FileSystemEntryRepository.FindEntry(version.PathInRepository);
      FileSet fileSet = new FileSet();
      fileSet.AddAll(entry.BreadthFirstFiles);
      CopyFiles(fileSet, directory);
    }
    #endregion

    private void CopyFiles(FileSet fileSet, Purl destiny)
    {
      if (!Infrastructure.FileSystem.IsDirectory(destiny.AsString))
      {
        Infrastructure.FileSystem.CreateDirectory(destiny.AsString);
      }
      int filesSoFar = 0;
      foreach (FileSystemFile file in fileSet.Files)
      {
        Purl fileDestiny = destiny.Join(file.Path.ChangeRoot(fileSet.FindCommonDirectory()));
        fileDestiny.CreateParentDirectory();
        Infrastructure.FileSystem.CopyFile(file.Purl.AsString, fileDestiny.AsString, false);
        filesSoFar++;
        DistributionDomainEvents.OnProgress(this, new FileCopyProgressEventArgs(filesSoFar / (double)fileSet.Count, file, destiny));
      }
    }
  }
}
