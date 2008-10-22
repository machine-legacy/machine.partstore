using System;
using System.Collections.Generic;
using System.IO;

using Machine.Partstore.Utility;
using Machine.Partstore.Domain.FileSystem;

namespace Machine.Partstore.Domain.Archiving
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
      ArchivingDomainEvents.OnProgress(this, new ArchiveFileProgressEventArgs(progress, _archive.Path, _currentEntry));
    }
  }
}
