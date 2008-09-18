using System;
using System.Collections.Generic;
using System.IO;

using ICSharpCode.SharpZipLib.Zip;

using DependencyStore.Domain.Core;
using DependencyStore.Domain.Core.Repositories.Impl;
using DependencyStore.Domain.SimpleCopying;
using DependencyStore.Utility;

namespace DependencyStore.Domain.Archiving
{
  public class ZipPackager
  {
    public const string ZipExtension = ".zip";

    private readonly Archive _archive;
    private long _totalBytes;
    private long _otherBytesSoFar;
    private ManifestEntry _currentEntry;

    public ZipPackager(Archive archive)
    {
      _archive = archive;
    }

    public FileSystemFile WriteZip(Purl path)
    {
      _totalBytes = _archive.UncompressedBytes;
      _otherBytesSoFar = 0;
      using (ZipOutputStream zip = OpenZipStream(path))
      {
        foreach (ManifestEntry entry in _archive.Entries)
        {
          using (Stream source = entry.FileAsset.OpenForReading())
          {
            _currentEntry = entry;
            ZipEntry zipEntry = new ZipEntry(entry.ArchivePath.AsString);
            zipEntry.DateTime = entry.FileAsset.ModifiedAt;
            zip.PutNextEntry(zipEntry);
            StreamHelper.Copy(source, zip, ReportProgress);
            zip.CloseEntry();
            _otherBytesSoFar += entry.UncompressedLength;
          }
        }
      }
      return FileSystemFileFactory.CreateFile(path);
    }

    private void ReportProgress(long bytesSoFar)
    {
      double progress = (_otherBytesSoFar + bytesSoFar) / (double)_totalBytes;
      DomainEvents.OnProgress(this, new ArchiveFileProgressEventArgs(progress, _currentEntry));
    }

    private static ZipOutputStream OpenZipStream(Purl path)
    {
      Stream stream = path.CreateFile();
      ZipOutputStream zip = new ZipOutputStream(stream);
      zip.SetLevel(5);
      return zip;
    }
  }
}