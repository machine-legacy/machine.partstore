using System;
using System.Collections.Generic;
using ICSharpCode.SharpZipLib.Zip;

namespace DependencyStore.Domain.Archiving
{
  public abstract class ManifestEntry
  {
    private readonly FileSystemPath _archivePath;

    public FileSystemPath ArchivePath
    {
      get { return _archivePath; }
    }

    public abstract long UncompressedLength
    {
      get;
    }

    protected ManifestEntry(FileSystemPath archivePath)
    {
      _archivePath = archivePath;
    }

    public override bool Equals(object obj)
    {
      if (obj is ManifestEntry)
      {
        return ((ManifestEntry)obj).ArchivePath.Equals(this.ArchivePath);
      }
      return false;
    }

    public override Int32 GetHashCode()
    {
      return this.ArchivePath.GetHashCode();
    }

    public override string ToString()
    {
      return String.Format("Entry<{0}, {1}>", this.ArchivePath);
    }
  }

  public class CompressedManifestEntry : ManifestEntry
  {
    private readonly ZipEntry _entry;

    public ZipEntry Entry
    {
      get { return _entry; }
    }

    public override long UncompressedLength
    {
      get { return _entry.Size; }
    }

    public CompressedManifestEntry(FileSystemPath archivePath, ZipEntry entry)
      : base(archivePath)
    {
      _entry = entry;
    }
  }

  public class LocalManifestEntry : ManifestEntry
  {
    private readonly FileSystemFile _file;

    public FileSystemFile File
    {
      get { return _file; }
    }

    public override long UncompressedLength
    {
      get { return _file.Length; }
    }

    public LocalManifestEntry(FileSystemPath archivePath, FileSystemFile file)
     : base(archivePath)
    {
      _file = file;
    }

    public override bool Equals(object obj)
    {
      if (obj is LocalManifestEntry)
      {
        return ((LocalManifestEntry)obj).File.Equals(this.File) && base.Equals(this);
      }
      return false;
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