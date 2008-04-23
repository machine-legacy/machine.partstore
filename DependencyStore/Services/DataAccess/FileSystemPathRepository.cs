using System;
using System.Collections.Generic;

using DependencyStore.Domain;
using Machine.Core.Services;

namespace DependencyStore.Services.DataAccess
{
  public class FileSystemPathRepository : IFileSystemPathRepository
  {
    #region IFileSystemPathRepository Members
    public IList<FileSystemPath> FindAll()
    {
      return new FileSystemPath[] {
        new BuildDirectoryPath(@"C:\Home\Source\Machine\Build"),
        new LibraryDirectoryPath(@"C:\Home\Source\JL\DependencyStore\Libraries"),
      };
    }
    #endregion
  }
}