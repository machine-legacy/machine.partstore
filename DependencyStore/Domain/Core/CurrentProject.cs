using System;
using System.Collections.Generic;

using DependencyStore.Domain.FileSystem;

namespace DependencyStore.Domain.Core
{
  public class CurrentProject : Project
  {
    private readonly ProjectManifestStore _manifests;
    private readonly IList<ProjectReference> _references;

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

    public CurrentProject(string name, Purl rootDirectory, Purl buildDirectory, Purl libraryDirectory, ProjectManifestStore manifests)
      : base(name, rootDirectory, buildDirectory, libraryDirectory)
    {
      _manifests = manifests;
      _references = Infrastructure.ProjectReferenceRepository.FindProjectReferences(this);
    }

    public ProjectReference AddReference(ArchivedProjectAndVersion archivedProjectAndVersion)
    {
      _manifests.AddManifestFor(archivedProjectAndVersion);
      ProjectReference projectReference = new HealthyProjectReference(this, archivedProjectAndVersion);
      _references.Add(projectReference);
      return projectReference;
    }

    public void UnpackageIfNecessary(RepositorySet repositorySet)
    {
      if (this.AreAllReferencesHealthy)
      {
        foreach (ProjectReference reference in _references)
        {
          reference.UnpackageIfNecessary(repositorySet);
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
