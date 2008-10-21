using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using DependencyStore.Domain.Core;

namespace DependencyStore.Application
{
  public class CurrentProjectState
  {
    private readonly bool _missingConfiguration;
    private readonly string _projectName;
    private readonly ReadOnlyCollection<ReferenceStatus> _references;

    public bool MissingConfiguration
    {
      get { return _missingConfiguration; }
    }

    public ICollection<ReferenceStatus> References
    {
      get { return _references; }
    }

    public string ProjectName
    {
      get { return _projectName; }
    }

    public CurrentProjectState(bool missingConfiguration)
      : this(missingConfiguration, String.Empty, new ReferenceStatus[0])
    {
    }

    public CurrentProjectState(string projectName, IEnumerable<ReferenceStatus> references)
      : this(false, projectName, references)
    {
    }

    public CurrentProjectState(bool missingConfiguration, string projectName, IEnumerable<ReferenceStatus> references)
    {
      _missingConfiguration = missingConfiguration;
      _projectName = projectName;
      _references = new ReadOnlyCollection<ReferenceStatus>(new List<ReferenceStatus>(references));
    }
  }
}