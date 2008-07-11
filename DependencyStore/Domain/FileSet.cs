using System;
using System.Collections.Generic;
using Machine.Container;

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

    public void AddAll(IEnumerable<SourceLocation> sources)
    {
      foreach (SourceLocation location in sources)
      {
        AddAll(location.FileEntry.BreadthFirstFiles);
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

    public string FindLongestCommonPrefix()
    {
      if (_files.Count == 0)
      {
        throw new YouFoundABugException("Can't find longest common prefix of empty set!");
      }
      if (_files.Count == 1)
      {
        return _files[0].Path.Full;
      }
      List<string> names = new List<string>();
      foreach (FileSystemFile file in _files)
      {
        names.Add(file.Path.Full);
      }
      names.Sort(delegate(string x, string y) { return x.Length.CompareTo(y.Length); });
      for (int i = 0; i < names[0].Length; ++i)
      {
        char c = names[0][i];
        for  (int j = 1; j < names.Count; ++j)
        {
          if (c != names[j][i])
          {
            return names[0].Substring(0, i);
          }
        }
      }
      return String.Empty;
    }
  }
}