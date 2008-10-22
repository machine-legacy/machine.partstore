using System;
using System.Collections.Generic;

using Machine.Partstore.Domain.FileSystem;

namespace Machine.Partstore.Domain.Core
{
  public class NewProjectVersion
  {
    private readonly ProjectVersionAdded _projectVersionAdded;

    public string ProjectName
    {
      get { return _projectVersionAdded.Project.Name; }
    }

    public Purl PathInRepository
    {
      get { return _projectVersionAdded.Version.PathInRepository; }
    }

    public string RepositoryAlias
    {
      get { return _projectVersionAdded.Version.RepositoryAlias; }
    }

    public FileSet FileSet
    {
      get { return _projectVersionAdded.Version.FileSet; }
    }

    public NewProjectVersion(ProjectVersionAdded projectVersionAdded)
    {
      _projectVersionAdded = projectVersionAdded;
    }

    public override string ToString()
    {
      return "NewVersion<" + this.ProjectName + " from " + this.FileSet.FindCommonDirectory() + " to " + this.RepositoryAlias + ">";
    }
  }
}