using System;
using System.Collections.Generic;

namespace DependencyStore.Domain.Core
{
  public class ReferenceCandidate
  {
    private readonly string _projectName;
    private readonly string _repositoryName;
    private readonly Tags _tags;
    private readonly DateTime _createdAtVersion;

    public string ProjectName
    {
      get { return _projectName; }
    }

    public string RepositoryName
    {
      get { return _repositoryName; }
    }

    public DateTime CreatedAtVersion
    {
      get { return _createdAtVersion; }
    }

    public Tags Tags
    {
      get { return _tags; }
    }

    public ReferenceCandidate(string repositoryName, string projectName, DateTime createdAtVersion, Tags tags)
    {
      _repositoryName = repositoryName;
      _projectName = projectName;
      _tags = tags;
      _createdAtVersion = createdAtVersion;
    }
  }
}
