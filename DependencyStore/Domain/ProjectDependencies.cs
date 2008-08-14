using System;
using System.Collections.Generic;

namespace DependencyStore.Domain
{
  public class ProjectDependencies
  {
    private readonly List<DependencyReference> _dependencies = new List<DependencyReference>();

    public List<DependencyReference> Dependencies
    {
      get { return _dependencies; }
    }

    public void AddDependency(DependencyReference depenency)
    {
      foreach (DependencyReference existing in Machine.Core.Utility.Enumerate.AndChange(_dependencies))
      {
        if (existing.IsSameProject(depenency))
        {
          _dependencies.Remove(existing);
        }
      }
      _dependencies.Add(depenency);
    }
  }
  public class DependencyReference : Machine.Core.ValueTypes.ClassTypeAsValueType
  {
    private readonly string _name;
    private readonly string _version;

    public string ProjectName
    {
      get { return _name; }
    }

    public string Version
    {
      get { return _version; }
    }

    public DependencyReference(string name, string version)
    {
      _name = name;
      _version = version;
    }

    public bool IsSameProject(DependencyReference another)
    {
      return another.ProjectName.Equals(this.ProjectName);
    }

    public bool IsSameProjectAndVersion(DependencyReference another)
    {
      return IsSameProjectAndVersion(another) && another.Version.Equals(this.Version);
    }
  }
}
