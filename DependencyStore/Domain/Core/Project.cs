using System;
using System.Collections.Generic;

using DependencyStore.Domain.FileSystem;

namespace DependencyStore.Domain.Core
{
  public class Project
  {
    private readonly string _name;
    private readonly Purl _rootDirectory;
    private readonly Purl _buildDirectory;
    private readonly Purl _libraryDirectory;

    public string Name
    {
      get { return _name; }
    }

    public Purl RootDirectory
    {
      get { return _rootDirectory; }
    }

    public bool HasBuildDirectory
    {
      get { return _buildDirectory != null; }
    }

    public Purl BuildDirectory
    {
      get
      {
        if (!this.HasBuildDirectory)
        {
          throw new InvalidOperationException("Project's build directory is missing!");
        }
        return _buildDirectory;
      }
    }

    public bool HasLibraryDirectory
    {
      get { return _libraryDirectory != null; }
    }

    public Purl LibraryDirectory
    {
      get
      {
        if (!this.HasLibraryDirectory)
        {
          throw new InvalidOperationException("Project's library directory is missing!");
        }
        return _libraryDirectory;
      }
    }

    public Purl DependencyPackageDirectoryFor(ArchivedProject dependency)
    {
      return this.LibraryDirectory.Join(dependency.Name);
    }

    public Project(string name, Purl rootDirectory, Purl buildDirectory, Purl libraryDirectory)
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
