using System;
using System.Collections.Generic;

using ICSharpCode.SharpZipLib.Zip;

using DependencyStore.Domain.Core;

namespace DependencyStore.Domain.Archiving
{
  public class ArchiveFactory
  {
    public static Archive ReadZip(Purl path)
    {
      ZipFile zip = new ZipFile(path.AsString);
      Archive archive = new Archive(path, zip);
      foreach (ZipEntry entry in zip)
      {
        if (!entry.IsDirectory)
        {
          Purl entryPath = new Purl(entry.Name);
          ArchivedFileInZip fileInZip = new ArchivedFileInZip(entryPath, zip, entry);
          ManifestEntry manifestEntry = new ManifestEntry(entryPath, fileInZip);
          archive.Add(manifestEntry);
        }
      }
      return archive;
    }
  }
}