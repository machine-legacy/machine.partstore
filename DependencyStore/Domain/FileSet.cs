using System.Collections.Generic;

using DependencyStore.Utility;

namespace DependencyStore.Domain
{
  public class FileSet
  {
    private readonly List<FileSystemFile> _files = new List<FileSystemFile>();

    public virtual void Add(FileSystemFile file)
    {
      if (!Contains(file))
      {
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

    public void Add(Location location)
    {
      AddAll(location.FileEntry.BreadthFirstFiles);
    }

    public void AddAll<TL>(IEnumerable<TL> locations) where TL : Location
    {
      foreach (TL location in locations)
      {
        Add(location);
      }
    }

    public void Remove(FileSystemFile file)
    {
      _files.Remove(file);
    }

    public bool Contains(FileSystemFile file)
    {
      return _files.Contains(file);
    }
    
    public IEnumerable<FileSystemFile> FindFilesNamed(string name)
    {
      foreach (FileSystemFile member in _files)
      {
        if (member.Name == name)
        {
          yield return member;
        }
      }
    }

    public IEnumerable<FileSystemFile> Files
    {
      get { return _files; }
    }

    public FileSystemPath FindCommonDirectory()
    {
      List<string> strings = new List<string>();
      foreach (FileSystemFile file in _files)
      {
        strings.Add(file.Path.Full);
      }
      return new FileSystemPath(StringHelper.FindLongestCommonPrefix(strings));
    }
  }
}