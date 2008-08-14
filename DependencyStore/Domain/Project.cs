using System;
using System.Collections.Generic;

using DependencyStore.Domain.Archiving;

namespace DependencyStore.Domain
{
  public class Project
  {
    private readonly string _name;
    private readonly SourceLocation _buildDirectory;

    public string Name
    {
      get { return _name; }
    }

    public SourceLocation BuildDirectory
    {
      get { return _buildDirectory; }
    }

    public Purl LibraryDirectory
    {
      get { return _buildDirectory.Path.Join(@"..\Libraries"); }
    }

    public Project(string name, SourceLocation buildDirectory)
    {
      _name = name;
      _buildDirectory = buildDirectory;
    }

    public Archive MakeArchive()
    {
      FileSet fileSet = this.BuildDirectory.ToFileSet();
      Purl fileRootDirectory = fileSet.FindCommonDirectory();
      Archive archive = new Archive();
      foreach (FileSystemFile file in fileSet.Files)
      {
        archive.Add(file.Path.ChangeRoot(fileRootDirectory), file);
      }
      return archive;
    }

    public override string ToString()
    {
      return "Project<" + this.Name + ">";
    }
  }
}
