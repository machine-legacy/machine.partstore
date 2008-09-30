using System;
using System.Xml.Serialization;

using DependencyStore.Domain.FileSystem;

namespace DependencyStore.Domain.Core
{
  public class ArchivedProjectVersion
  {
    private VersionNumber _versionNumber;
    private string _repositoryAlias;
    private Tags _tags;
    private Purl _pathInRepository;
    private FileSet _fileSet;

    public VersionNumber Number
    {
      get { return _versionNumber; }
      set { _versionNumber = value; }
    }

    public string RepositoryAlias
    {
      get { return _repositoryAlias; }
      set { _repositoryAlias = value; }
    }

    public Tags Tags
    {
      get { return _tags; }
      set { _tags = value; }
    }

    [XmlIgnore]
    public Purl PathInRepository
    {
      get { return _pathInRepository; }
      set { _pathInRepository = value; }
    }

    [XmlIgnore]
    public FileSet FileSet
    {
      get { return _fileSet; }
      set { _fileSet = value; }
    }

    public string CreatedAtVersion
    {
      get { return DateTimeToUniqueString(_versionNumber.TimeStamp); }
    }

    protected ArchivedProjectVersion()
    {
    }

    protected ArchivedProjectVersion(VersionNumber version, string archiveFileName, Purl pathInRepository, Tags tags)
    {
      _versionNumber = version;
      _repositoryAlias = archiveFileName;
      _pathInRepository = pathInRepository;
      _tags = tags;
    }

    public static ArchivedProjectVersion Create(Repository repository, ArchivedProject project, Tags tags)
    {
      VersionNumber version = new VersionNumber();
      string repositoryAlias = project.Name + "-" + DateTimeToUniqueString(version.TimeStamp);
      return new ArchivedProjectVersion(version, repositoryAlias, repository.RootPath.Join(repositoryAlias), tags);
    }

    private static string DateTimeToUniqueString(DateTime when)
    {
      return when.ToString("yyyyMMdd-HHmmssf");
    }

    public override string ToString()
    {
      return "ArchivedVersion<" + this.RepositoryAlias + ">";
    }
  }
}