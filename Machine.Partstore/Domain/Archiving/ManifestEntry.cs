using System;
using System.Collections.Generic;

using Machine.Partstore.Domain.FileSystem;

namespace Machine.Partstore.Domain.Archiving
{
  public class ManifestEntry
  {
    private readonly Purl _archivePath;
    private readonly FileAsset _fileAsset;

    public Purl ArchivePath
    {
      get { return _archivePath; }
    }

    public FileAsset FileAsset
    {
      get { return _fileAsset; }
    }

    public virtual long UncompressedLength
    {
      get { return _fileAsset.LengthInBytes; }
    }

    public ManifestEntry(Purl archivePath, FileAsset fileAsset)
    {
      _archivePath = archivePath;
      _fileAsset = fileAsset;
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
      return String.Format("Entry<{0}>", this.ArchivePath);
    }
  }
}