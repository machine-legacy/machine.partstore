using System;
using System.Collections.Generic;
using System.IO;

using DependencyStore.Utility;

using ICSharpCode.SharpZipLib.Zip;

namespace DependencyStore.Domain.Archiving
{
  public class Archive
  {
    public const string ZipExtension = ".zip";
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

    public void Add(Purl archivePath, FileSystemFile file)
    {
      _entries.Add(new LocalManifestEntry(archivePath, file));
    }

    public void Add(ZipEntry zipEntry)
    {
      _entries.Add(new CompressedManifestEntry(new Purl(zipEntry.Name), zipEntry));
    }

    public FileSystemFile WriteZip(Purl path)
    {
      long totalBytes = this.UncompressedBytes;
      long otherBytesSoFar = 0;
      using (ZipOutputStream zip = OpenZipStream(path))
      {
        foreach (LocalManifestEntry entry in _entries)
        {
          using (Stream source = entry.File.OpenForReading())
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

    public FileSet ToFileSet()
    {
      FileSet fileSet = new FileSet();
      foreach (CompressedManifestEntry entry in _entries)
      {
        fileSet.Add(new FileSystemFile(entry.ArchivePath));
      }
      return fileSet;
    }

    public static Archive ReadZip(Purl path)
    {
      Archive archive = new Archive();
      using (ZipFile zip = new ZipFile(path.AsString))
      {
        foreach (ZipEntry entry in zip)
        {
          if (!entry.IsDirectory)
          {
            archive.Add(entry);
          }
        }
      }
      return archive;
    }
  }
}
