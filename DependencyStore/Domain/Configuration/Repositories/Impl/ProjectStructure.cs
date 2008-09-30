using System;

using DependencyStore.Domain.FileSystem;
using Machine.Core.Services;

namespace DependencyStore.Domain.Configuration.Repositories.Impl
{
  public class ProjectStructure
  {
    private readonly IFileSystem _fileSystem;
    private readonly Purl _root;

    public ProjectStructure(IFileSystem fileSystem,  Purl root)
    {
      _fileSystem = fileSystem;
      _root = root;
    }

    public ProjectConfiguration InferProjectConfiguration()
    {
      ProjectConfiguration configuration = new ProjectConfiguration();
      configuration.Name = _root.Name;
      configuration.Root = FindRootDirectory();
      configuration.Build = FindBuildDirectory();
      configuration.Library = FindLibraryDirectory();
      configuration.EnsureValid();
      return configuration;
    }

    protected RootDirectoryConfiguration FindRootDirectory()
    {
      return new RootDirectoryConfiguration(_root);
    }

    protected BuildDirectoryConfiguration FindBuildDirectory()
    {
      Purl build = SelectFirstValidDirectory("Build", "Bin");
      if (build == null)
      {
        return null;
      }
      return new BuildDirectoryConfiguration(build);
    }

    protected LibraryDirectoryConfiguration FindLibraryDirectory()
    {
      Purl build = SelectFirstValidDirectory("Libraries", "Lib");
      if (build == null)
      {
        return null;
      }
      return new LibraryDirectoryConfiguration(build);
    }

    protected Purl SelectFirstValidDirectory(params string[] relativeCandidates)
    {
      foreach (string candidate in relativeCandidates)
      {
        Purl path = _root.Join(candidate);
        if (_fileSystem.IsDirectory(path.AsString))
        {
          return path;
        }
      }
      return null;
    }
  }
}