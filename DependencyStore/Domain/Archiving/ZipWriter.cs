using System;
using System.Collections.Generic;
using System.IO;

using DependencyStore.Utility;

using ICSharpCode.SharpZipLib.Zip;

namespace DependencyStore.Domain.Archiving
{
  public class ZipArchiveWriter
  {
    public const string ZipExtension = ".zip";

    private readonly Archive _archive;

    public ZipArchiveWriter(Archive archive)
    {
      _archive = archive;
    }

    public FileSystemFile WriteZip(Purl path)
    {
      long totalBytes = _archive.UncompressedBytes;
      long otherBytesSoFar = 0;
      using (ZipOutputStream zip = OpenZipStream(path))
      {
        foreach (ManifestEntry entry in _archive.Entries)
        {
          using (Stream source = entry.FileAsset.OpenForReading())
          {
            ZipEntry zipEntry = new ZipEntry(entry.ArchivePath.AsString);
            zip.PutNextEntry(zipEntry);
            StreamHelper.Copy(source, zip, delegate(long bytesSoFar) {
                                                                       double progress = (otherBytesSoFar + bytesSoFar) / (double)totalBytes;
                                                                       DomainEvents.OnProgress(this, new ZipFileProgressEventArgs(progress, entry));
            });
            zip.CloseEntry();
            otherBytesSoFar += entry.UncompressedLength;
          }
        }
      }
      return new FileSystemFile(path);
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