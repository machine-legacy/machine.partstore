using System;
using System.Collections.Generic;

using DependencyStore.Domain;
using DependencyStore.Services.DataAccess;

namespace DependencyStore.Services
{
  public class Controller : IController
  {
    private readonly IFileSystemPathRepository _fileSystemPathRepository;
    private readonly IFileSystemEntryRepository _fileSystemEntryRepository;

    public Controller(IFileSystemEntryRepository fileSystemEntryRepository, IFileSystemPathRepository fileSystemPathRepository)
    {
      _fileSystemEntryRepository = fileSystemEntryRepository;
      _fileSystemPathRepository = fileSystemPathRepository;
    }

    #region IController Members
    public void Show()
    {
      InclusionExclusionRules rules = new InclusionExclusionRules();
      rules.AddExclusion(@"^.svn$");
      rules.AddInclusion(@"^.*\.dll$");
      rules.AddInclusion(@"^.*\.pdb$");
      foreach (FileSystemPath path in _fileSystemPathRepository.FindAll())
      {
        FileSystemEntry entry = _fileSystemEntryRepository.FindEntry(path);
        Console.WriteLine(path);
        Console.WriteLine(entry);
      }
    }

    public void Clear()
    {
    }

    public void Refresh()
    {
    }
    #endregion
  }
}
