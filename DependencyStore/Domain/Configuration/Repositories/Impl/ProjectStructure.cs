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
      return new BuildDirectoryConfiguration(_root.Join("Build"));
    }

    protected LibraryDirectoryConfiguration FindLibraryDirectory()
    {
      return new LibraryDirectoryConfiguration(_root.Join("Libraries"));
    }
  }
}