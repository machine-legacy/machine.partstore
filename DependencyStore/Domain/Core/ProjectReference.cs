using System;
using System.Collections.Generic;

namespace DependencyStore.Domain.Core
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
    private readonly ArchivedProject _dependency;
    private readonly ArchivedProjectVersion _version;
    private readonly ProjectDependencyDirectory _installed;

    public HealthyProjectReference(Project parentProject, ArchivedProject dependency, ArchivedProjectVersion version)
    {
      _parentProject = parentProject;
      _dependency = dependency;
      _version = version;
      _installed = new ProjectDependencyDirectory(_parentProject, _dependency);
    }

    private ProjectDependencyDirectory Installed
    {
      get { return _installed; }
    }

    public override void UnpackageIfNecessary(RepositorySet repositorySet)
    {
      if (this.Status.IsOlderVersionInstalled)
      {
        this.Installed.UpdateInstalledVersion(repositorySet, _version);
      }
    }

    public override ReferenceStatus Status
    {
      get { return ReferenceStatus.Create(_dependency, _version, _installed); }
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
      throw new InvalidOperationException("You can't Unpackage an unhealthy reference!");
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
