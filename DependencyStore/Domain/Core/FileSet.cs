using System.Collections.Generic;

using DependencyStore.Utility;

namespace DependencyStore.Domain.Core
{
  public class FileSet
  {
    private readonly List<FileAsset> _files = new List<FileAsset>();

    public bool IsEmpty
    {
      get { return _files.Count == 0; }
    }

    public virtual void Add(FileAsset file)
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

    public void AddAll(IEnumerable<FileAsset> files)
    {
      foreach (FileAsset file in files)
      {
        Add(file);
      }
    }

    public void Remove(FileAsset file)
    {
      _files.Remove(file);
    }

    public bool Contains(FileAsset file)
    {
      return _files.Contains(file);
    }
    
    public IEnumerable<FileAsset> FindFilesNamed(string name)
    {
      foreach (FileAsset member in _files)
      {
        if (member.Purl.Name == name)
        {
          yield return member;
        }
      }
    }

    public IEnumerable<FileAsset> Files
    {
      get { return _files; }
    }

    public int Count
    {
      get { return _files.Count; }
    }

    public Purl FindCommonDirectory()
    {
      List<string> strings = new List<string>();
      foreach (FileAsset file in _files)
      {
        strings.Add(file.Purl.AsString);
      }
      return new Purl(StringHelper.FindLongestCommonPrefix(strings));
    }

    public void SortByModifiedAt()
    {
      /* Most recently modified first... */
      _files.Sort(delegate(FileAsset a, FileAsset b) {
        return b.ModifiedAt.CompareTo(a.ModifiedAt);
      });
    }
  }
}
