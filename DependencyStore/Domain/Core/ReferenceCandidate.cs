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
    private readonly DateTime _createdAt;

    public string ProjectName
    {
      get { return _projectName; }
    }

    public string RepositoryName
    {
      get { return _repositoryName; }
    }

    public DateTime CreatedAt
    {
      get { return _createdAt; }
    }

    public string PrettyAge
    {
      get { return TimeSpanHelper.ToPrettyString(DateTime.UtcNow - this.CreatedAt); }
    }

    public Tags Tags
    {
      get { return _tags; }
    }

    public ReferenceCandidate(string repositoryName, string projectName, DateTime createdAt, Tags tags)
    {
      _repositoryName = repositoryName;
      _projectName = projectName;
      _tags = tags;
      _createdAt = createdAt;
    }
  }
}
