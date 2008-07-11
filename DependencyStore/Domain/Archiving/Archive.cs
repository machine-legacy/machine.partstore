using System;
using System.Collections.Generic;
using System.IO;

using ICSharpCode.SharpZipLib.Zip;

namespace DependencyStore.Domain.Archiving
{
  public class Archive
  {
    private readonly List<ManifestEntry> _entries = new List<ManifestEntry>();

    public void Add(FileSystemFile file, FileSystemPath archivePath)
    {
      _entries.Add(new ManifestEntry(file, archivePath));
    }

    public FileSystemFile WriteZip(string path)
    {
      using (Stream stream = File.Create(path))
      {
        ZipOutputStream zip = new ZipOutputStream(stream);
        zip.SetLevel(5);
        foreach (ManifestEntry entry in _entries)
        {
          using (Stream source = entry.File.OpenForReading())
          {
            ZipEntry zipEntry = new ZipEntry(entry.ArchivePath.Full);
            zip.PutNextEntry(zipEntry);
            StreamHelper.Copy(source, zip);
          }
        }
      }
      return new FileSystemFile(new FileSystemPath(path));
    }
  }
  public static class StreamHelper
  {
    public static void Copy(Stream source, Stream destiny)
    {
      while (true)
      {
        byte[] buffer = new byte[65536];
        int bytes = source.Read(buffer, 0, buffer.Length);
        if (bytes < 0)
        {
          break;
        }
        destiny.Write(buffer, 0, bytes);
      }
    }
  }
}
