using System;
using System.Collections.Generic;

using DependencyStore.Domain.Archiving;

namespace DependencyStore.Domain
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
      get { return _buildDirectory; }
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
          throw new InvalidOperationException("Not allowed to use this project's library!");
        }
        return _libraryDirectory;
      }
    }

    public Project(string name, Purl rootDirectory, Purl buildDirectory, Purl libraryDirectory)
    {
      _name = name;
      _rootDirectory = rootDirectory;
      _buildDirectory = buildDirectory;
      _libraryDirectory = libraryDirectory;
    }

    public Archive MakeArchive()
    {
      throw new NotImplementedException();
      /*
      FileSet fileSet = this.BuildDirectory.ToFileSet();
      Purl fileRootDirectory = fileSet.FindCommonDirectory();
      Archive archive = new Archive();
      foreach (FileSystemFile file in fileSet.Files)
      {
        archive.Add(file.Path.ChangeRoot(fileRootDirectory), file);
      }
      return archive;
      */
    }

    public override string ToString()
    {
      return "Project<" + this.Name + ">";
    }
  }
}
