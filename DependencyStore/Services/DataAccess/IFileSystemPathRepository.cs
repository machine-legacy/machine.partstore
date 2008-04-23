using System;
using System.Collections.Generic;

using DependencyStore.Domain;

namespace DependencyStore.Services.DataAccess
{
  public interface IFileSystemPathRepository
  {
    IList<FileSystemPath> FindAll();
  }
}
