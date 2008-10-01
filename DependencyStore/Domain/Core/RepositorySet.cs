using System;
using System.Collections.Generic;

using Machine.Core;

namespace DependencyStore.Domain.Core
{
  public class RepositorySet
  {
    private readonly List<Repository> _repositories;

    public Repository DefaultRepository
    {
      get
      {
        if (_repositories.Count == 0)
        {
          throw new InvalidOperationException("No default repository!");
        }
        return _repositories[0];
      }
    }

    public RepositorySet(IEnumerable<Repository> repositories)
    {
      _repositories = new List<Repository>(repositories);
    }

    public ArchivedProject FindProject(string name)
    {
      foreach (Repository repository in _repositories)
      {
        ArchivedProject project = repository.FindProject(name);
        if (project != null)
        {
          return project;
        }
      }
      return null;
    }

    public IEnumerable<ReferenceCandidate> FindAllReferenceCandidates()
    {
      List<ReferenceCandidate> candidates = new List<ReferenceCandidate>();
      foreach (Repository repository in _repositories)
      {
        candidates.AddRange(repository.FindAllReferenceCandidates());
      }
      return candidates;
    }

    public ArchivedProjectAndVersion FindArchivedProjectAndVersion(ReferenceCandidate referenceCandidate)
    {
      ArchivedProject project = FindProject(referenceCandidate.ProjectName);
      if (project == null) throw new YouFoundABugException();
      ArchivedProjectVersion version = project.FindVersionByNumber(referenceCandidate.VersionNumber);
      if (version == null) throw new YouFoundABugException();
      return new ArchivedProjectAndVersion(project, version);
    }

    public void Refresh()
    {
      foreach (Repository repository in _repositories)
      {
        repository.Refresh();
      }
    }
  }
}
