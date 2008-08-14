using System;
using System.Collections.Generic;
using System.IO;
using DependencyStore.Domain;
using Machine.Core.Services;
using Machine.Core.Utility;

namespace DependencyStore.Services.DataAccess.Impl
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
      foreach (string fileName in _fileSystem.GetFiles(project.LibraryDirectory.AsString, "*.projref"))
      {
        ProjectManifest manifest = ReadManifest(fileName);
        manifests.Add(manifest);
      }
      return manifests;
    }

    public void SaveProjectManifest()
    {
    }
    #endregion

    private ProjectManifest ReadManifest(string path)
    {
      using (StreamReader stream = new StreamReader(_fileSystem.OpenFile(path)))
      {
        return XmlSerializationHelper.DeserializeString<ProjectManifest>(stream.ReadToEnd());
      }
    }
  }
}
