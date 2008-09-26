using System;
using System.Collections.Generic;
using System.IO;

using Machine.Core.Services;
using Machine.Core.Utility;

using DependencyStore.Domain.FileSystem;

namespace DependencyStore.Domain.Core.Repositories.Impl
{
  public class ProjectManifestRepository : IProjectManifestRepository
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(ProjectManifestRepository));
    private static readonly XmlSerializer<ProjectManifest> _serializer = new XmlSerializer<ProjectManifest>();
    private readonly Dictionary<Purl, ProjectManifestStore> _cache = new Dictionary<Purl, ProjectManifestStore>();
    private readonly IFileSystem _fileSystem;

    public ProjectManifestRepository(IFileSystem fileSystem)
    {
      _fileSystem = fileSystem;
    }

    #region IProjectManifestRepository Members
    public ProjectManifestStore FindProjectManifestStore(Purl path)
    {
      _log.Info("Loading: " + path.AsString);
      if (_cache.ContainsKey(path))
      {
        return _cache[path];
      }
      ProjectManifestStore manifestStore = new ProjectManifestStore(path, ReadManifests(path));
      _cache[manifestStore.RootDirectory] = manifestStore;
      return manifestStore;
    }

    public ProjectManifestStore FindProjectManifestStore(Project project)
    {
      if (project.HasLibraryDirectory)
      {
        return FindProjectManifestStore(project.LibraryDirectory);
      }
      return new ProjectManifestStore(project.LibraryDirectory, new List<ProjectManifest>());
    }

    public void SaveProjectManifestStore(ProjectManifestStore projectManifestStore)
    {
      _log.Info("Saving: " + projectManifestStore.RootDirectory.AsString);
      foreach (ProjectManifest manifest in projectManifestStore)
      {
        Purl manifestPath = projectManifestStore.RootDirectory.Join(manifest.FileName);
        SaveProjectManifest(manifest, manifestPath);
      }
    }
    #endregion

    private IList<ProjectManifest> ReadManifests(Purl directory)
    {
      List<ProjectManifest> manifests = new List<ProjectManifest>();
      if (_fileSystem.IsDirectory(directory.AsString))
      {
        foreach (string fileName in _fileSystem.GetFiles(directory.AsString, "*." + ProjectManifest.Extension))
        {
          manifests.Add(ReadProjectManifest(new Purl(fileName)));
        }
      }
      return manifests;
    }

    private ProjectManifest ReadProjectManifest(Purl path)
    {
      using (StreamReader stream = new StreamReader(_fileSystem.OpenFile(path.AsString)))
      {
        ProjectManifest manifest = _serializer.DeserializeString(stream.ReadToEnd());
        if (!manifest.IsAcceptableFileName(path))
        {
          throw new InvalidOperationException("Project reference manifest and project name should match: " + path);
        }
        return manifest;
      }
    }

    private void SaveProjectManifest(ProjectManifest manifest, Purl path)
    {
      using (StreamWriter stream = new StreamWriter(_fileSystem.CreateFile(path.AsString)))
      {
        stream.Write(_serializer.Serialize(manifest));
      }
    }
  }
}
