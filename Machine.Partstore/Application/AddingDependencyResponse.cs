using System;
using System.Collections.Generic;

namespace DependencyStore.Application
{
  public class AddingDependencyResponse
  {
    private readonly bool _noMatchingProject;
    private readonly bool _ambiguousProjectName;

    public bool NoMatchingProject
    {
      get { return _noMatchingProject; }
    }

    public bool AmbiguousProjectName
    {
      get { return _ambiguousProjectName; }
    }

    public bool Success
    {
      get { return !_noMatchingProject && !_ambiguousProjectName; }
    }

    public AddingDependencyResponse(bool noMatchingProject, bool ambiguousProjectName)
    {
      _noMatchingProject = noMatchingProject;
      _ambiguousProjectName = ambiguousProjectName;
    }
  }
}