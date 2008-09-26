using System;
using System.Xml.Serialization;

using DependencyStore.Domain.FileSystem;

namespace DependencyStore.Domain.Distribution
{
  public class ArchivedProjectVersion
  {
    private DateTime _createdAt;
    private string _repositoryAlias;
    private Tags _tags;
    private Purl _pathInRepository;
    private FileSet _fileSet;

    public DateTime CreatedAt
    {
      get { return _createdAt; }
      set { _createdAt = value; }
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
      get { return DateTimeToUniqueString(_createdAt); }
    }

    protected ArchivedProjectVersion()
    {
    }

    protected ArchivedProjectVersion(DateTime createdAt, string archiveFileName, Purl pathInRepository, Tags tags)
    {
      _createdAt = createdAt;
      _repositoryAlias = archiveFileName;
      _pathInRepository = pathInRepository;
      _tags = tags;
    }

    public static ArchivedProjectVersion Create(Repository repository, ArchivedProject project, Tags tags)
    {
      DateTime createdAt = DateTime.Now;
      string repositoryAlias = project.Name + "-" + DateTimeToUniqueString(createdAt);
      return new ArchivedProjectVersion(createdAt, repositoryAlias, repository.RootPath.Join(repositoryAlias), tags);
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