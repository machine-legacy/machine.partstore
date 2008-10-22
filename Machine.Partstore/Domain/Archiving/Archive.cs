using System;
using System.Collections.Generic;

using ICSharpCode.SharpZipLib.Zip;

using DependencyStore.Domain.FileSystem;

namespace DependencyStore.Domain.Archiving
{
  public class Archive : IDisposable
  {
    private readonly List<ManifestEntry> _entries = new List<ManifestEntry>();
    private readonly ZipFile _zipFile;
    private readonly Purl _path;

    public Purl Path
    {
      get { return _path; }
    }

    public long UncompressedBytes
    {
      get
      {
        long total = 0;
        foreach (ManifestEntry entry in _entries)
        {
          total += entry.UncompressedLength;
        }
        return total;
      }
    }

    public IEnumerable<ManifestEntry> Entries
    {
      get { return _entries; }
    }

    public IEnumerable<FileAsset> FileAssets
    {
      get
      {
        foreach (ManifestEntry entry in _entries)
        {
          yield return entry.FileAsset;
        }
      }
    }

    public Archive()
    {
    }

    public Archive(Purl path, ZipFile zipFile)
    {
      _path = path;
      _zipFile = zipFile;
    }

    public void Add(ManifestEntry manifestEntry)
    {
      _entries.Add(manifestEntry);
    }

    public void Add(Purl archivePath, FileSystemFile file)
    {
      Add(new ManifestEntry(archivePath, file));
    }

    public FileSet ToFileSet()
    {
      FileSet fileSet = new FileSet();
      foreach (FileAsset file in this.FileAssets)
      {
        fileSet.Add(file);
      }
      return fileSet;
    }

    #region IDisposable Members
    public void Dispose()
    {
      if (_zipFile != null)
      {
        _zipFile.Close();
      }
    }
    #endregion
  }
}
