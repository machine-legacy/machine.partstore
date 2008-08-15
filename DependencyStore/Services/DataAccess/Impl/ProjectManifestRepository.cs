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
        manifests.Add(ReadProjectManifest(new Purl(fileName)));
      }
      return manifests;
    }

    public ProjectManifest ReadProjectManifest(Purl path)
    {
      using (StreamReader stream = new StreamReader(_fileSystem.OpenFile(path.AsString)))
      {
        return XmlSerializationHelper.DeserializeString<ProjectManifest>(stream.ReadToEnd());
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
