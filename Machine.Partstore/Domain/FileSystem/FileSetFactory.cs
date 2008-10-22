using System;
using System.Collections.Generic;

namespace Machine.Partstore.Domain.FileSystem
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
