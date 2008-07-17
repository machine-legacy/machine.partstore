using System;
using System.Collections.Generic;

namespace DependencyStore.Domain
{
  public class Project
  {
    private readonly string _name;
    private readonly SourceLocation _location;

    public string Name
    {
      get { return _name; }
    }

    public SourceLocation Location
    {
      get { return _location; }
    }

    public Project(string name, SourceLocation location)
    {
      _name = name;
      _location = location;
    }
  }
}
