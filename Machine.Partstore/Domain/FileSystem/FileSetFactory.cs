using System;
using System.Collections.Generic;

namespace DependencyStore.Domain.FileSystem
{
  public static class FileSetFactory
  {
    public static FileSet CreateFileSetFrom(Purl directory)
    {
      FileSystemEntry entry = Infrastructure.FileSystemEntryRepository.FindEntry(directory);
      FileSet fileSet = new FileSet();
      fileSet.AddAll(entry.BreadthFirstFiles);
      return fileSet;
    }
  }
}
