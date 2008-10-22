using System;
using System.Collections.Generic;

namespace Machine.Partstore.Domain.Core
{
  public abstract class ProjectReference
  {
    protected ProjectReference()
    {
    }

    public abstract void UnpackageIfNecessary(RepositorySet repositorySet);

    public abstract ReferenceStatus Status
    {
      get;
    }
  }
  public class HealthyProjectReference : ProjectReference
  {
    private readonly Project _parentProject;
    private readonly ArchivedProjectAndVersion _archivedProjectAndVersion;
    private readonly ProjectDependencyDirectory _installed;

    public HealthyProjectReference(Project parentProject, ArchivedProjectAndVersion archivedProjectAndVersion)
    {
      _parentProject = parentProject;
      _archivedProjectAndVersion = archivedProjectAndVersion;
      _installed = new ProjectDependencyDirectory(_parentProject, _archivedProjectAndVersion.Project);
    }

    private ProjectDependencyDirectory Installed
    {
      get { return _installed; }
    }

    public override void UnpackageIfNecessary(RepositorySet repositorySet)
    {
      if (this.Status.IsOlderVersionInstalled)
      {
        this.Installed.UpdateInstalledVersion(repositorySet, _archivedProjectAndVersion.Version);
      }
    }

    public override ReferenceStatus Status
    {
      get { return ReferenceStatus.Create(_archivedProjectAndVersion, _installed); }
    }
  }
  public class BrokenProjectReference : ProjectReference
  {
    private readonly ReferenceStatus _status;

    protected BrokenProjectReference(ReferenceStatus status)
    {
      _status = status;
    }

    public override void UnpackageIfNecessary(RepositorySet repositorySet)
    {
      throw new InvalidOperationException("You can't unpackage an unhealthy reference!");
    }

    public override ReferenceStatus Status
    {
      get { return _status; }
    }

    public static ProjectReference MissingProject(ProjectManifest manifest)
    {
      return new BrokenProjectReference(ReferenceStatus.CreateMissingProject(manifest));
    }

    public static ProjectReference MissingVersion(ProjectManifest manifest)
    {
      return new BrokenProjectReference(ReferenceStatus.CreateMissingVersion(manifest));
    }
  }
}
