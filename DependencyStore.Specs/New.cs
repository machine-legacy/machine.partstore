using System;
using System.Collections.Generic;

using DependencyStore.Domain.Configuration;
using DependencyStore.Domain.Core;
using DependencyStore.Domain.FileSystem;

namespace DependencyStore
{
  public static class Ager
  {
    private static DateTime _now = DateTime.Parse("11/14/1985 10:00AM");

    public static DateTime Next()
    {
      return _now += TimeSpan.FromMinutes(10.0);
    }
  }
  public class NewCreators
  {
    public CurrentProjectCreator CurrentProject(ProjectManifestStore manifestStore, RepositorySet repositorySet)
    {
      return new CurrentProjectCreator(manifestStore, repositorySet);
    }

    public CurrentProjectCreator CurrentProject()
    {
      return CurrentProject(ManifestStore(), RepositorySet());
    }

    public ProjectManifestCreator Manifest(string name)
    {
      return new ProjectManifestCreator(name, Version());
    }

    public VersionNumber Version()
    {
      return new VersionNumber(Ager.Next());
    }

    public ProjectManifestStore ManifestStore(params ProjectManifest[] manifests)
    {
      ProjectManifestStoreCreator creator = new ProjectManifestStoreCreator(RandomPurl());
      foreach (ProjectManifest manifest in manifests)
      {
        creator.With(manifest);
      }
      return creator;
    }

    public ConfigurationCreator Configuration()
    {
      return new ConfigurationCreator();
    }

    public ArchivedProjectCreator ArchivedProject(string name)
    {
      return new ArchivedProjectCreator(name);
    }

    public ArchivedProjectVersionCreator ArchivedProjectVersion(Purl repositoryRoot, string name)
    {
      return new ArchivedProjectVersionCreator(repositoryRoot, name);
    }

    public RepositoryCreator Repository()
    {
      return new RepositoryCreator(RandomPurl());
    }

    public RepositorySetCreator RepositorySet()
    {
      return new RepositorySetCreator();
    }

    public Purl RandomPurl()
    {
      return new Purl(Guid.NewGuid().ToString());
    }
  }

  public abstract class Creator<T>
  {
    private T _creation;

    public T Creation
    {
      get
      {
        if (Equals(default(T), _creation))
        {
          _creation = Create();
        }
        return _creation;
      }
    }

    public abstract T Create();

    public static implicit operator T(Creator<T> creator)
    {
      return creator.Creation;
    }
  }

  public class ProjectManifestStoreCreator : Creator<ProjectManifestStore>
  {
    private readonly Purl _path;
    private readonly List<ProjectManifest> _manifests = new List<ProjectManifest>();

    public ProjectManifestStoreCreator With(ProjectManifest manifest)
    {
      _manifests.Add(manifest);
      return this;
    }

    public ProjectManifestStoreCreator(Purl path)
    {
      _path = path;
    }

    public override ProjectManifestStore Create()
    {
      return new ProjectManifestStore(_path, _manifests);
    }
  }

  public class ProjectManifestCreator : Creator<ProjectManifest>
  {
    private readonly string _name;
    private readonly VersionNumber _version;

    public ProjectManifestCreator(string name, VersionNumber version)
    {
      _name = name;
      _version = version;
    }

    public override ProjectManifest Create()
    {
      return new ProjectManifest(_name, _version);
    }
  }

  public class CurrentProjectCreator : Creator<CurrentProject>
  {
    private readonly ProjectManifestStore _projectManifestStore;
    private readonly RepositorySet _repositorySet;
    private string _name = "TestProject";

    public CurrentProjectCreator(ProjectManifestStore projectManifestStore, RepositorySet repositorySet)
    {
      _projectManifestStore = projectManifestStore;
      _repositorySet = repositorySet;
    }

    public override CurrentProject Create()
    {
      return new CurrentProject(_name, null, null, null, _repositorySet, _projectManifestStore);
    }
  }

  public class ConfigurationCreator : Creator<DependencyStoreConfiguration>
  {
    public override DependencyStoreConfiguration Create()
    {
      DependencyStoreConfiguration configuration = new DependencyStoreConfiguration();
      configuration.ConfigurationPath = new Purl(String.Empty);
      return configuration;
    }
  }

  public class ArchivedProjectCreator : Creator<ArchivedProject>
  {
    private readonly string _name;
    private readonly List<ArchivedProjectVersion> _versions = new List<ArchivedProjectVersion>();

    public ArchivedProjectCreator(string name)
    {
      _name = name;
    }

    public ArchivedProjectCreator With(ArchivedProjectVersion version)
    {
      _versions.Add(version);
      return this;
    }

    public override ArchivedProject Create()
    {
      ArchivedProject project = new ArchivedProject();
      project.Name = _name;
      foreach (ArchivedProjectVersion version in _versions)
      {
        project.Versions.AddRange(_versions);
      }
      return project;
    }
  }

  public class ArchivedProjectVersionCreator : Creator<ArchivedProjectVersion>
  {
    private readonly Purl _repositoryRoot;
    private readonly string _projectName;

    public ArchivedProjectVersionCreator(Purl repositoryRoot, string projectName)
    {
      _repositoryRoot = repositoryRoot;
      _projectName = projectName;
    }

    public override ArchivedProjectVersion Create()
    {
      return ArchivedProjectVersion.Create(_repositoryRoot, _projectName, Tags.None);
    }
  }

  public class RepositoryCreator : Creator<Repository>
  {
    private readonly Purl _path;

    public RepositoryCreator(Purl path)
    {
      _path = path;
    }

    public override Repository Create()
    {
      Repository repository = new Repository();
      repository.RootPath = _path;
      return repository;
    }
  }

  public class RepositorySetCreator : Creator<RepositorySet>
  {
    private readonly List<Repository> _repositories = new List<Repository>();

    public RepositorySetCreator With(Repository repository)
    {
      _repositories.Add(repository);
      return this;
    }

    public override RepositorySet Create()
    {
      return new RepositorySet(_repositories);
    }
  }
}