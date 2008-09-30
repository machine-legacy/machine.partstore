using System;
using System.Collections.Generic;

using Machine.Core.Utility;

namespace DependencyStore.Domain.Core
{
  public class ReferenceCandidate
  {
    private readonly string _projectName;
    private readonly string _repositoryName;
    private readonly Tags _tags;
    private readonly VersionNumber _versionId;

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
      get { return _versionId; }
    }

    public string PrettyAge
    {
      get { return TimeSpanHelper.ToPrettyString(DateTime.UtcNow - _versionId.TimeStamp); }
    }

    public Tags Tags
    {
      get { return _tags; }
    }

    public ReferenceCandidate(string repositoryName, string projectName, VersionNumber versionId, Tags tags)
    {
      _repositoryName = repositoryName;
      _projectName = projectName;
      _tags = tags;
      _versionId = versionId;
    }
  }
}
