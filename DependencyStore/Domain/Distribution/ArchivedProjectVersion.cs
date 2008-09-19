using System;
using System.Xml.Serialization;

using DependencyStore.Domain.Archiving;
using DependencyStore.Domain.Core;

namespace DependencyStore.Domain.Distribution
{
  public class ArchivedProjectVersion
  {
    private DateTime _createdAt;
    private string _repositoryAlias;
    private Purl _pathInRepository;

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

    [XmlIgnore]
    public Purl PathInRepository
    {
      get { return _pathInRepository; }
      set { _pathInRepository = value; }
    }

    public string CreatedAtVersion
    {
      get { return DateTimeToUniqueString(_createdAt); }
    }

    protected ArchivedProjectVersion()
    {
    }

    protected ArchivedProjectVersion(DateTime createdAt, string archiveFileName, Purl pathInRepository)
    {
      _createdAt = createdAt;
      _repositoryAlias = archiveFileName;
      _pathInRepository = pathInRepository;
    }

    public static ArchivedProjectVersion Create(ArchivedProject project, Repository repository)
    {
      DateTime createdAt = DateTime.Now;
      string repositoryAlias = project.Name + "-" + DateTimeToUniqueString(createdAt);
      return new ArchivedProjectVersion(createdAt, repositoryAlias, repository.RootPath.Join(repositoryAlias));
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