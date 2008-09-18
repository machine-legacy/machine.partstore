using System;
using System.Xml.Serialization;

using DependencyStore.Domain.Archiving;
using DependencyStore.Domain.Core;

namespace DependencyStore.Domain.Distribution
{
  public class ArchivedProjectVersion
  {
    private DateTime _createdAt;
    private string _archiveFileName;
    private Purl _archivePath;

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

    protected ArchivedProjectVersion(DateTime createdAt, string archiveFileName, Purl archivePath)
    {
      _createdAt = createdAt;
      _archiveFileName = archiveFileName;
      _archivePath = archivePath;
    }

    public static ArchivedProjectVersion Create(ArchivedProject project, Repository repository)
    {
      DateTime createdAt = DateTime.Now;
      string archiveFileName = project.Name + "-" + DateTimeToUniqueString(createdAt) + ZipPackager.ZipExtension;
      return new ArchivedProjectVersion(createdAt, archiveFileName, repository.RootPath.Join(archiveFileName));
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