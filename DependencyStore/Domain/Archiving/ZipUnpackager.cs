using System;
using System.Collections.Generic;
using System.IO;

using DependencyStore.Domain.SimpleCopying;
using DependencyStore.Utility;
using DependencyStore.Domain.Core;

namespace DependencyStore.Domain.Archiving
{
  public class ZipUnpackager
  {
    private readonly Archive _archive;
    private ManifestEntry _currentEntry;
    private long _totalBytes;
    private long _otherBytesSoFar;

    public ZipUnpackager(Archive archive)
    {
      _archive = archive;
    }

    public void UnpackageZip(Purl destination)
    {
      foreach (ManifestEntry manifestEntry in _archive.Entries)
      {
        _totalBytes += manifestEntry.UncompressedLength;
      }
      foreach (ManifestEntry manifestEntry in _archive.Entries)
      {
        FileAsset zippedAsset = manifestEntry.FileAsset;
        Purl destinationFile = destination.Join(zippedAsset.Purl);
        using (Stream source = zippedAsset.OpenForReading())
        {
          destinationFile.CreateParentDirectory();
          using (Stream destiny = destinationFile.CreateFile())
          {
            _currentEntry = manifestEntry;
            StreamHelper.Copy(source, destiny, ReportProgress);
          }
          _otherBytesSoFar += manifestEntry.UncompressedLength;
        }
      }
    }

    private void ReportProgress(long bytesSoFar)
    {
      double progress = (_otherBytesSoFar + bytesSoFar) / (double)_totalBytes;
      DomainEvents.OnProgress(this, new ArchiveFileProgressEventArgs(progress, _currentEntry));
    }
  }
}
