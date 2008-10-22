using System;
using System.Xml.Serialization;

using Machine.Partstore.Domain.FileSystem;

namespace Machine.Partstore.Domain.Core
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

    protected ArchivedProjectVersion()
    {
    }

    protected ArchivedProjectVersion(VersionNumber version, string repositoryAlias, Purl pathInRepository, Tags tags)
    {
      _versionNumber = version;
      _repositoryAlias = repositoryAlias;
      _pathInRepository = pathInRepository;
      _tags = tags;
    }

    public static ArchivedProjectVersion Create(Purl repositoryRoot, string projectName, Tags tags)
    {
      VersionNumber version = new VersionNumber();
      string repositoryAlias = projectName + "-" + version.AsString;
      Purl pathInRepository = repositoryRoot.Join(repositoryAlias);
      return new ArchivedProjectVersion(version, repositoryAlias, pathInRepository, tags);
    }

    public static ArchivedProjectVersion Create(Repository repository, ArchivedProject project, Tags tags)
    {
      return Create(repository.RootPath, project.Name, tags);
    }

    public override string ToString()
    {
      return "ArchivedVersion<" + this.RepositoryAlias + ">";
    }
  }
}