using System;
using System.Collections.Generic;

namespace Machine.Partstore.Domain.Core
{
  public class CurrentProject : Project
  {
    private readonly ProjectManifestStore _manifests;
    private readonly RepositorySet _repositorySet;
    private IList<ProjectReference> _references;

    public RepositorySet RepositorySet
    {
      get { return _repositorySet; }
    }

    public ProjectManifestStore Manifests
    {
      get { return _manifests; }
    }

    public ICollection<ProjectReference> References
    {
      get
      {
        if (_references == null)
        {
          _references = new List<ProjectReference>(ProjectReferenceFactory.FindProjectReferences(_repositorySet, this, _manifests));
        }
        return _references;
      }
    }

    public IEnumerable<ReferenceStatus> ReferenceStatuses
    {
      get
      {
        foreach (ProjectReference reference in this.References)
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
    }

    public ProjectReference AddReference(ArchivedProjectAndVersion archivedProjectAndVersion)
    {
      _manifests.AddManifestFor(archivedProjectAndVersion);
      ProjectReference projectReference = new HealthyProjectReference(this, archivedProjectAndVersion);
      _references = null;
      return projectReference;
    }

    public void UnpackageIfNecessary()
    {
      if (this.AreAllReferencesHealthy)
      {
        foreach (ProjectReference reference in this.References)
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
