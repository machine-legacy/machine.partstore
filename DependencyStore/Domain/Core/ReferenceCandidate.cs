using System;
using System.Collections.Generic;

namespace DependencyStore.Domain.Core
{
  public class ReferenceCandidate
  {
    private readonly string _projectName;
    private readonly string _repositoryName;
    private readonly Tags _tags;
    private readonly VersionNumber _versionNumber;

    public string ProjectName
    {
      get { return _projectName; }
    }

    public string RepositoryName
    {
      get { return _repositoryName; }
    }

    public VersionNumber VersionNumber
    {
      get { return _versionNumber; }
    }

    public string PrettyAge
    {
      get { return _versionNumber.PrettyString; }
    }

    public Tags Tags
    {
      get { return _tags; }
    }

    public ReferenceCandidate(string projectName)
      : this(null, projectName, null, Tags.None)
    {
    }

    public ReferenceCandidate(string repositoryName, string projectName, VersionNumber versionNumber, Tags tags)
    {
      _repositoryName = repositoryName;
      _projectName = projectName;
      _tags = tags;
      _versionNumber = versionNumber;
    }

    public override bool Equals(object obj)
    {
      ReferenceCandidate other = obj as ReferenceCandidate;
      if (other == null)
      {
        return false;
      }
      if (!EitherIsNullOrTheyAreEqual(this.RepositoryName, other.RepositoryName))
      {
        return false;
      }
      return this.ProjectName.Equals(other.ProjectName, StringComparison.InvariantCultureIgnoreCase);
    }
    
    private static bool EitherIsNullOrTheyAreEqual(string v1, string v2)
    {
      if (v1 != null && v2 != null)
      {
        return v1.Equals(v2, StringComparison.InvariantCultureIgnoreCase);
      }
      return true;
    }

    public override int GetHashCode()
    {
      Int32 hashCode = _projectName.GetHashCode();
      if (_repositoryName != null)
      {
        hashCode ^=_repositoryName.GetHashCode();
      }
      return hashCode;
    }
  }
}
