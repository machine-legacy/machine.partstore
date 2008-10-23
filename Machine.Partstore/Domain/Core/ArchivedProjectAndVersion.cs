using System;
using System.Collections.Generic;

namespace Machine.Partstore.Domain.Core
{
  public class ProjectFromRepository
  {
    private readonly Repository _repository;
    private readonly ArchivedProject _project;

    public ProjectFromRepository(Repository repository, ArchivedProject project)
    {
      _repository = repository;
      _project = project;
    }

    public Repository Repository
    {
      get { return _repository; }
    }

    public ArchivedProject Project
    {
      get { return _project; }
    }

    public override string ToString()
    {
      return "ProjectFromRepository<" + this.Repository + ", " + this.Project + ">";
    }
  }
  public class ArchivedProjectAndVersion
  {
    private readonly Repository _repository;
    private readonly ArchivedProject _project;
    private readonly ArchivedProjectVersion _version;

    public Repository Repository
    {
      get { return _repository; }
    }

    public ArchivedProject Project
    {
      get { return _project; }
    }

    public ArchivedProjectVersion Version
    {
      get { return _version; }
    }

    public ArchivedProjectAndVersion(ProjectFromRepository projectFromRepository, ArchivedProjectVersion version)
      : this(projectFromRepository.Repository, projectFromRepository.Project, version)
    {
    }

    public ArchivedProjectAndVersion(Repository repository, ArchivedProject project, ArchivedProjectVersion version)
    {
      _repository = repository;
      _project = project;
      _version = version;
    }

    public override bool Equals(object obj)
    {
      ArchivedProjectAndVersion other = obj as ArchivedProjectAndVersion;
      if (other != null)
      {
        return other.Project.Equals(this.Project) && other.Version.Equals(this.Version);
      }
      return false;
    }

    public override Int32 GetHashCode()
    {
      return this.Project.GetHashCode() ^ this.Version.GetHashCode();
    }

    public override string ToString()
    {
      return "ArchivedProjectAndVersion<" + this.Repository + ", " + this.Project + ", " + this.Version + ">";
    }
  }
}
