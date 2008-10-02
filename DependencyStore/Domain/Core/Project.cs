using System;
using System.Collections.Generic;

using DependencyStore.Domain.FileSystem;

namespace DependencyStore.Domain.Core
{
  public class Project
  {
    private readonly string _name;
    private readonly ProjectDirectory _rootDirectory;
    private readonly ProjectDirectory _buildDirectory;
    private readonly ProjectDirectory _libraryDirectory;

    public string Name
    {
      get { return _name; }
    }

    public ProjectDirectory RootDirectory
    {
      get { return _rootDirectory; }
    }

    public ProjectDirectory BuildDirectory
    {
      get { return _buildDirectory; }
    }

    public ProjectDirectory LibraryDirectory
    {
      get { return _libraryDirectory; }
    }

    public Purl DependencyPackageDirectoryFor(ArchivedProject dependency)
    {
      return this.LibraryDirectory.Path.Join(dependency.Name);
    }

    public Project(string name, ProjectDirectory rootDirectory, ProjectDirectory buildDirectory, ProjectDirectory libraryDirectory)
    {
      _name = name;
      _rootDirectory = rootDirectory;
      _buildDirectory = buildDirectory;
      _libraryDirectory = libraryDirectory;
    }

    public override string ToString()
    {
      return "Project<" + this.Name + ">";
    }
  }
}
