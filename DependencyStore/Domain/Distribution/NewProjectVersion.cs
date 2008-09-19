using System;
using System.Collections.Generic;

using DependencyStore.Domain.Core;

namespace DependencyStore.Domain.Distribution
{
  public class NewProjectVersion
  {
    private readonly ArchivedProject _project;
    private readonly ArchivedProjectVersion _version;
    private readonly FileSet _fileSet;

    public string ProjectName
    {
      get { return _project.Name; }
    }

    public Purl PathInRepository
    {
      get { return _version.PathInRepository; }
    }

    public string RepositoryAlias
    {
      get { return _version.RepositoryAlias; }
    }

    public FileSet FileSet
    {
      get { return _fileSet; }
    }

    public NewProjectVersion(ArchivedProject project, ArchivedProjectVersion version, FileSet fileSet)
    {
      _project = project;
      _version = version;
      _fileSet = fileSet;
    }
  }
}