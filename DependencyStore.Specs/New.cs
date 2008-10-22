using System;
using System.Collections.Generic;

using DependencyStore.Domain.Configuration;
using DependencyStore.Domain.Core;
using DependencyStore.Domain.FileSystem;

namespace DependencyStore
{
  public class NewCreators
  {
    public CurrentProjectCreator CurrentProject(ProjectManifestStore manifestStore)
    {
      return new CurrentProjectCreator(manifestStore);
    }

    public CurrentProjectCreator CurrentProject()
    {
      return CurrentProject(ManifestStore());
    }

    public ProjectManifest Manifest()
    {
      return new ProjectManifest();
    }

    public ProjectManifestStore ManifestStore()
    {
      return new ProjectManifestStoreCreator(RandomPurl());
    }

    public ConfigurationCreator Configuration()
    {
      return new ConfigurationCreator();
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
    private string _name = "TestProject";

    public CurrentProjectCreator(ProjectManifestStore projectManifestStore)
    {
      _projectManifestStore = projectManifestStore;
    }

    public override CurrentProject Create()
    {
      return new CurrentProject(_name, null, null, null, null, _projectManifestStore);
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
}