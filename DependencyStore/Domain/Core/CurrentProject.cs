using System;
using System.Collections.Generic;

namespace DependencyStore.Domain.Core
{
  public class CurrentProject : Project
  {
    private readonly ProjectManifestStore _manifests;
    private readonly RepositorySet _repositorySet;
    private readonly IList<ProjectReference> _references;

    public RepositorySet RepositorySet
    {
      get { return _repositorySet; }
    }

    public ProjectManifestStore Manifests
    {
      get { return _manifests; }
    }

    public IEnumerable<ProjectReference> References
    {
      get { return _references; }
    }

    public IEnumerable<ReferenceStatus> ReferenceStatuses
    {
      get
      {
        foreach (ProjectReference reference in _references)
        {
          yield return reference.Status;
        }
      }
    }

    public bool AreAllReferencesHealthy
    {
      get
      {
        foreach (ReferenceStatus status in this.ReferenceStatuses)
        {
          if (!status.IsHealthy)
          {
            return false;
          }
        }
        return true;
      }
    }

    public CurrentProject(string name, ProjectDirectory rootDirectory, ProjectDirectory buildDirectory, ProjectDirectory libraryDirectory, RepositorySet repositorySet, ProjectManifestStore manifests)
      : base(name, rootDirectory, buildDirectory, libraryDirectory)
    {
      _repositorySet = repositorySet;
      _manifests = manifests;
      _references = new List<ProjectReference>(ProjectReferenceFactory.FindProjectReferences(repositorySet, this, manifests));
    }

    public ProjectReference AddReference(ArchivedProjectAndVersion archivedProjectAndVersion)
    {
      _manifests.AddManifestFor(archivedProjectAndVersion);
      ProjectReference projectReference = new HealthyProjectReference(this, archivedProjectAndVersion);
      _references.Add(projectReference);
      return projectReference;
    }

    public void UnpackageIfNecessary()
    {
      if (this.AreAllReferencesHealthy)
      {
        foreach (ProjectReference reference in _references)
        {
          reference.UnpackageIfNecessary(_repositorySet);
        }
      }
      else
      {
        throw new InvalidOperationException("Not all references are healthy!");
      }
    }

    public void AddNewVersion(Repository repository, Tags tags)
    {
      repository.AddNewVersion(this, tags);
    }
  }
}
