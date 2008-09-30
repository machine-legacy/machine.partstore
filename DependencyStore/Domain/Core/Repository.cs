using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using DependencyStore.Domain.FileSystem;

namespace DependencyStore.Domain.Core
{
  public class Repository
  {
    public static readonly IRepositoryAccessStrategy AccessStrategy = new MirroredRepositoryAccessStrategy();
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

    public string Name
    {
      get { return _rootPath.Name; }
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

    private void AddProject(ArchivedProject project)
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
    /*
    public ArchivedProjectVersion FindProjectVersion(ProjectManifest manifest)
    {
      ArchivedProject project = FindProject(manifest);
      if (project == null)
      {
        return null;
      }
      return project.FindVersionInManifest(manifest);
    }
    */
    public void AddNewVersion(Project project, Tags tags)
    {
      ArchivedProject archivedProject = FindOrCreateProject(project);
      ArchivedProjectVersion version = ArchivedProjectVersion.Create(this, archivedProject, tags);
      version.FileSet = FileSetFactory.CreateFileSetFrom(project.BuildDirectory);
      archivedProject.AddVersion(version);
    }

    public IEnumerable<ReferenceCandidate> FindAllReferenceCandidates()
    {
      foreach (ArchivedProject project in _projects)
      {
        yield return new ReferenceCandidate(this.Name, project.Name, project.LatestVersion.CreatedAt, project.LatestVersion.Tags);
      }
    }
  }
}
