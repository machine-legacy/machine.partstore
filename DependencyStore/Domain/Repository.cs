using System;
using System.Collections.Generic;

namespace DependencyStore.Domain
{
  public class Repository
  {
    private readonly List<ArchivedProject> _projects = new List<ArchivedProject>();

    public List<ArchivedProject> Projects
    {
      get { return _projects; }
    }

    public bool IsEmpty
    {
      get { return _projects.Count == 0; }
    }

    public ArchivedProject FindProject(string name)
    {
      foreach (ArchivedProject existing in _projects)
      {
        if (existing.Name.Equals(name))
        {
          return existing;
        }
      }
      return null;
    }

    public void AddProject(ArchivedProject project)
    {
      if (FindProject(project.Name) != null)
      {
        throw new InvalidOperationException();
      }
      _projects.Add(project);
    }
  }
  public class ArchivedProject
  {
    private readonly List<ArchivedProjectVersion> _versions = new List<ArchivedProjectVersion>();
    private string _name;

    public string Name
    {
      get { return _name; }
      set { _name = value; }
    }

    public List<ArchivedProjectVersion> Versions
    {
      get { return _versions; }
    }

    public ArchivedProject()
    {
    }

    public ArchivedProject(string name)
    {
      _name = name;
    }

    public ArchivedProjectVersion FindVersion(string version)
    {
      foreach (ArchivedProjectVersion existing in _versions)
      {
        if (existing.Version.Equals(version))
        {
          return existing;
        }
      }
      return null;
    }

    public void AddVersion(ArchivedProjectVersion version)
    {
      if (FindVersion(version.Version) != null)
      {
        throw new InvalidOperationException();
      }
      _versions.Add(version);
    }
  }
  public class ArchivedProjectVersion
  {
    private string _version;
    private DateTime _createdAt;

    public string Version
    {
      get { return _version; }
      set { _version = value; }
    }

    public DateTime CreatedAt
    {
      get { return _createdAt; }
      set { _createdAt = value; }
    }

    public ArchivedProjectVersion()
    {
    }

    public ArchivedProjectVersion(DateTime createdAt, string version)
    {
      _createdAt = createdAt;
      _version = version;
    }
  }
}
