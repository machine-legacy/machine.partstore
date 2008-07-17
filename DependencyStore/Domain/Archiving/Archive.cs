using System;
using System.Collections.Generic;
using System.IO;

using DependencyStore.Utility;

using ICSharpCode.SharpZipLib.Zip;

namespace DependencyStore.Domain.Archiving
{
  public class Archive
  {
    private readonly List<ManifestEntry> _entries = new List<ManifestEntry>();

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

    public void Add(FileSystemFile file, FileSystemPath archivePath)
    {
      _entries.Add(new ManifestEntry(file, archivePath));
    }

    public FileSystemFile WriteZip(FileSystemPath path)
    {
      long totalBytes = this.UncompressedBytes;
      long otherBytesSoFar = 0;
      using (Stream stream = path.CreateFile())
      {
        ZipOutputStream zip = new ZipOutputStream(stream);
        zip.SetLevel(5);
        foreach (ManifestEntry entry in _entries)
        {
          using (Stream source = entry.File.OpenForReading())
          {
            ZipEntry zipEntry = new ZipEntry(entry.ArchivePath.AsString);
            zip.PutNextEntry(zipEntry);
            StreamHelper.Copy(source, zip, delegate(long bytesSoFar) {
              double progress = (otherBytesSoFar + bytesSoFar) / (double)totalBytes;
              DomainEvents.OnProgress(this, new ProgressEventArgs("Writing Zip", "", progress));
            });
            zip.CloseEntry();
            otherBytesSoFar += entry.UncompressedLength;
          }
        }
      }
      return new FileSystemFile(path);
    }
  }
}
