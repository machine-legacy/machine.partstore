using System;
using System.Collections.Generic;

namespace DependencyStore.Domain
{
  public class LatestFiles
  {
    private readonly List<FileSystemFile> _files = new List<FileSystemFile>();
    
    public FileSystemFile FindExistingByName(FileSystemFile file)
    {
      foreach (FileSystemFile member in _files)
      {
        if (member.Name == file.Name)
        {
          return member;
        }
      }
      return null;
    }

    public void Add(FileSystemFile file)
    {
      FileSystemFile existing = FindExistingByName(file);
      if (existing == null)
      {
        _files.Add(file);
      }
      else if (file.IsNewerThan(existing))
      {
        _files.Remove(existing);
        _files.Add(file);
      }
    }

    public void AddAll(IEnumerable<FileSystemFile> files)
    {
      foreach (FileSystemFile file in files)
      {
        Add(file);
      }
    }

    public void AddAll(IEnumerable<SourceLocation> sources)
    {
      foreach (SourceLocation location in sources)
      {
        AddAll(location.FileEntry.BreadthFirstFiles);
      }
    }

    public IEnumerable<FileSystemFile> Files
    {
      get { return _files; }
    }
  }
}
