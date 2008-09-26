using System;

using DependencyStore.Domain.FileSystem;

namespace DependencyStore.Domain.Configuration.Repositories.Impl
{
  public class ProjectStructure
  {
    private readonly Purl _root;

    public ProjectStructure(Purl root)
    {
      _root = root;
    }

    public Purl FindRootDirectory()
    {
      return _root;
    }

    public Purl FindBuildDirectory()
    {
      return _root.Join("Build");
    }

    public Purl FindLibraryDirectory()
    {
      return _root.Join("Libraries");
    }
  }
}