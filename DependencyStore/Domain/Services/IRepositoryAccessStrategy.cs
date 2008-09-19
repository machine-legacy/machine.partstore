using System;
using System.Collections.Generic;

using DependencyStore.Domain.Archiving;
using DependencyStore.Domain.Core;
using DependencyStore.Domain.Distribution;

namespace DependencyStore.Domain.Services
{
  public interface IRepositoryAccessStrategy
  {
    void AddVersionToRepository(NewProjectVersion newProjectVersion);
  }
  public class ArchiveRepositoryAccessStrategy : IRepositoryAccessStrategy
  {
    #region IRepositoryAdditionStrategy Members
    public void AddVersionToRepository(NewProjectVersion newProjectVersion)
    {
      using (Archive archive = MakeArchiveFor(newProjectVersion))
      {
        ZipPackager writer = new ZipPackager(archive);
        writer.WriteZip(newProjectVersion.ArchivePath);
      }
    }
    #endregion

    private static Archive MakeArchiveFor(NewProjectVersion newProjectVersion)
    {
      Archive archive = new Archive();
      foreach (FileSystemFile file in newProjectVersion.Files)
      {
        archive.Add(file.Path.ChangeRoot(newProjectVersion.CommonRootDirectory), file);
      }
      return archive;
    }
  }
}