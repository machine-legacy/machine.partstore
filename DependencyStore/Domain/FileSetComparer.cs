using System;
using System.Collections.Generic;

namespace DependencyStore.Domain
{
  public class FileSetComparer : IEqualityComparer<FileAsset>
  {
    public FileSet Compare(FileSet a, FileSet b)
    {
      HashSet<FileAsset> filesInB = new HashSet<FileAsset>(b.Files, this);
      HashSet<FileAsset> filesInA = new HashSet<FileAsset>(this);
      FileSet changes = new FileSet();
      foreach (FileAsset file in a.Files)
      {
        if (!filesInB.Contains(file))
        {
          changes.Add(file);
        }
        else
        {
          filesInB.Remove(file);
        }
      }
      changes.AddAll(filesInB);
      return changes;
    }

    #region IEqualityComparer<FileAsset> Members
    public bool Equals(FileAsset x, FileAsset y)
    {
      return x.Purl.Equals(y.Purl) && x.ModifiedAt.Equals(y.ModifiedAt);
    }

    public Int32 GetHashCode(FileAsset obj)
    {
      return obj.Purl.GetHashCode() ^ obj.ModifiedAt.GetHashCode();
    }
    #endregion
  }
}