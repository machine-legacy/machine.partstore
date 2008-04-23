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

    public IEnumerable<FileSystemFile> Files
    {
      get { return _files; }
    }
  }
}
