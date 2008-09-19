using System;
using System.Collections.Generic;

using DependencyStore.Domain.Archiving;
using DependencyStore.Domain.Core;

namespace DependencyStore.Domain.Distribution
{
  public class MirroredRepositoryAccessStrategy : IRepositoryAccessStrategy
  {
    #region IRepositoryAccessStrategy Members
    public void CommitVersionToRepository(NewProjectVersion newProjectVersion)
    {
      Purl destiny = newProjectVersion.PathInRepository;
      CopyFiles(newProjectVersion.FileSet, destiny);
    }

    public void CheckoutVersionFromRepository(ArchivedProjectVersion version, Purl directory)
    {
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
        Infrastructure.FileSystem.CopyFile(file.Purl.AsString, fileDestiny.AsString, false);
        filesSoFar++;
        DistributionDomainEvents.OnProgress(this, new FileCopyProgressEventArgs(filesSoFar / (double)fileSet.Count, file, destiny));
      }
    }
  }
}
