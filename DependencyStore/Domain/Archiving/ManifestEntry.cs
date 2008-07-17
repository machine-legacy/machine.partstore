using System;
using System.Collections.Generic;

namespace DependencyStore.Domain.Archiving
{
  public class ManifestEntry
  {
    private readonly FileSystemFile _file;
    private readonly FileSystemPath _archivePath;

    public FileSystemFile File
    {
      get { return _file; }
    }

    public FileSystemPath ArchivePath
    {
      get { return _archivePath; }
    }

    public long UncompressedLength
    {
      get { return _file.Length; }
    }

    public ManifestEntry(FileSystemFile file, FileSystemPath archivePath)
    {
      _file = file;
      _archivePath = archivePath;
    }

    public override bool Equals(object obj)
    {
      if (obj is ManifestEntry)
      {
        return ((ManifestEntry)obj).File.Equals(this.File) && ((ManifestEntry)obj).ArchivePath.Equals(this.ArchivePath);
      }
      return base.Equals(obj);
    }

    public override Int32 GetHashCode()
    {
      return this.File.GetHashCode() ^ this.ArchivePath.GetHashCode();
    }

    public override string ToString()
    {
      return String.Format("Entry<{0}, {1}>", this.File, this.ArchivePath);
    }
  }
}