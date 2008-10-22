using System;

using Machine.Partstore.Domain.FileSystem;
using Machine.Core.Services;

namespace Machine.Partstore.Domain.Configuration.Repositories.Impl
{
  public class ProjectStructure
  {
    private readonly Purl _root;

    public ProjectStructure(Purl root)
    {
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
        if (Infrastructure.FileSystem.IsDirectory(path.AsString))
        {
          return path;
        }
      }
      return null;
    }
  }
}