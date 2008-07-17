using System;
using System.Collections.Generic;
using DependencyStore.Domain.Archiving;

namespace DependencyStore.Domain
{
  public class Project
  {
    private readonly string _name;
    private readonly SourceLocation _location;

    public string Name
    {
      get { return _name; }
    }

    public SourceLocation Location
    {
      get { return _location; }
    }

    public Project(string name, SourceLocation location)
    {
      _name = name;
      _location = location;
    }

    public Archive MakeArchive()
    {
      FileSet fileSet = this.Location.ToFileSet();
      FileSystemPath fileRootDirectory = fileSet.FindCommonDirectory();
      Archive archive = new Archive();
      foreach (FileSystemFile file in fileSet.Files)
      {
        archive.Add(file, file.Path.Chroot(fileRootDirectory));
      }
      return archive;
    }
  }
}
