using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using DependencyStore.Domain.Core;

namespace DependencyStore.Domain.Distribution
{
  public class Repository
  {
    public static readonly IRepositoryAccessStrategy AccessStrategy = new ArchiveRepositoryAccessStrategy();
    private readonly List<ArchivedProject> _projects = new List<ArchivedProject>();
    private Purl _rootPath;

    public List<ArchivedProject> Projects
    {
      get { return _projects; }
    }

    [XmlIgnore]
    public Purl RootPath
    {
      get { return _rootPath; }
      set { _rootPath = value; }
    }

    public bool IsEmpty
    {
      get { return _projects.Count == 0; }
    }

    public ArchivedProject FindOrCreateProject(Project project)
    {
      ArchivedProject archived = FindProject(project.Name);
      if (archived == null)
      {
        archived = new ArchivedProject(project.Name);
        AddProject(archived);
      }
      return archived;
    }

    public ArchivedProject FindProject(Project project)
    {
      return FindProject(project.Name);
    }

    public ArchivedProject FindProject(string name)
    {
      foreach (ArchivedProject existing in _projects)
      {
        if (existing.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
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

    public ArchivedProject FindProject(ProjectManifest manifest)
    {
      return FindProject(manifest.ProjectName);
    }

    public ArchivedProjectVersion FindProjectVersion(ProjectManifest manifest)
    {
      ArchivedProject project = FindProject(manifest);
      if (project == null)
      {
        return null;
      }
      return project.FindVersionInManifest(manifest);
    }
  }
}
