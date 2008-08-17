using System;
using System.Xml.Serialization;

using DependencyStore.Domain.Archiving;

namespace DependencyStore.Domain.Repositories
{
  public class ArchivedProjectVersion
  {
    private string _packager;
    private DateTime _createdAt;
    private string _archiveFileName;
    private Purl _archivePath;

    public string Packager
    {
      get { return _packager; }
      set { _packager = value; }
    }

    public DateTime CreatedAt
    {
      get { return _createdAt; }
      set { _createdAt = value; }
    }

    public string ArchiveFileName
    {
      get { return _archiveFileName; }
      set { _archiveFileName = value; }
    }

    [XmlIgnore]
    public Purl ArchivePath
    {
      get { return _archivePath; }
      set { _archivePath = value; }
    }

    public string CreatedAtVersion
    {
      get { return DateTimeToUniqueString(_createdAt); }
    }

    protected ArchivedProjectVersion()
    {
    }

    protected ArchivedProjectVersion(DateTime createdAt, string archiveFileName, string packager)
    {
      _createdAt = createdAt;
      _archiveFileName = archiveFileName;
      _packager = packager;
    }

    public static ArchivedProjectVersion Create(ArchivedProject project)
    {
      DateTime createdAt = DateTime.Now;
      string archiveFileName = project.Name + "-" + DateTimeToUniqueString(createdAt) + ZipPackager.ZipExtension;
      return new ArchivedProjectVersion(createdAt, archiveFileName, "Nobody");
    }

    private static string DateTimeToUniqueString(DateTime when)
    {
      return when.ToString("yyyyMMdd-HHmmssf");
    }

    public override string ToString()
    {
      return "ArchivedVersion<" + this.ArchiveFileName + ">";
    }
  }
}