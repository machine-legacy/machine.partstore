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

    public ReferenceCandidate(string repositoryName, string projectName, VersionNumber versionNumber, Tags tags)
    {
      _repositoryName = repositoryName;
      _projectName = projectName;
      _tags = tags;
      _versionNumber = versionNumber;
    }
  }
}
