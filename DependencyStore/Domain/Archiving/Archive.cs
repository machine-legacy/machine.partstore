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

    public void Add(FileSystemFile file, FileSystemPath archivePath)
    {
      _entries.Add(new ManifestEntry(file, archivePath));
    }

    public FileSystemFile WriteZip(FileSystemPath path)
    {
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
            StreamHelper.Copy(source, zip);
            zip.CloseEntry();
          }
        }
      }
      return new FileSystemFile(path);
    }
  }
}
