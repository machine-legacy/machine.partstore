using System;
using System.Collections.Generic;
using System.IO;

using ICSharpCode.SharpZipLib.Zip;

using DependencyStore.Domain.Core;

namespace DependencyStore.Domain.Archiving
{
  public class ArchivedFileInZip : ArchivedFile
  {
    private readonly ZipFile _zipFile;
    private readonly ZipEntry _zipEntry;

    public ArchivedFileInZip(Purl path, ZipFile zipFile, ZipEntry zipEntry)
      : base(path)
    {
      _zipFile = zipFile;
      _zipEntry = zipEntry;
    }

    public override Stream OpenForReading()
    {
      return _zipFile.GetInputStream(_zipEntry);
    }

    public override long LengthInBytes
    {
      get { return _zipEntry.Size; }
    }

    public override DateTime ModifiedAt
    {
      get { return _zipEntry.DateTime; }
    }

    public override string ToString()
    {
      return String.Format(@"ArchivedFile<{0}>", this.Purl);
    }
  }
}