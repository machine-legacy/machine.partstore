using System;
using System.Collections.Generic;
using System.IO;

using Machine.Core.Services;
using Machine.Core.Utility;

using DependencyStore.Domain.Core;

namespace DependencyStore.Domain.Repositories.Repositories.Impl
{
  public class ProjectManifestRepository : IProjectManifestRepository
  {
    private readonly IFileSystem _fileSystem;

    public ProjectManifestRepository(IFileSystem fileSystem)
    {
      _fileSystem = fileSystem;
    }

    #region IProjectManifestRepository Members
    public IList<ProjectManifest> FindProjectManifests(Project project)
    {
      List<ProjectManifest> manifests = new List<ProjectManifest>();
      if (project.HasLibraryDirectory)
      {
        foreach (string fileName in _fileSystem.GetFiles(project.LibraryDirectory.AsString, "*." + ProjectManifest.Extension))
        {
          manifests.Add(ReadProjectManifest(new Purl(fileName)));
        }
      }
      return manifests;
    }

    public ProjectManifest ReadProjectManifest(Purl path)
    {
      using (StreamReader stream = new StreamReader(_fileSystem.OpenFile(path.AsString)))
      {
        ProjectManifest manifest = XmlSerializationHelper.DeserializeString<ProjectManifest>(stream.ReadToEnd());
        if (!manifest.IsAcceptableFileName(path))
        {
          throw new InvalidOperationException("Project reference manifest and project name should match: " + path);
        }
        return manifest;
      }
    }

    public void SaveProjectManifest(ProjectManifest manifest, Purl path)
    {
      using (StreamWriter stream = new StreamWriter(_fileSystem.CreateFile(path.AsString)))
      {
        stream.Write(XmlSerializationHelper.Serialize(manifest));
      }
    }
    #endregion
  }
}
