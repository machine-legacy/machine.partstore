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

    public Purl ArchivePath
    {
      get { return _version.ArchivePath; }
    }

    public string RepositoryAlias
    {
      get { return _version.RepositoryAlias; }
    }

    public Purl CommonRootDirectory
    {
      get { return _fileSet.FindCommonDirectory(); }
    }

    public IEnumerable<FileAsset> Files
    {
      get { return _fileSet.Files; }
    }

    public NewProjectVersion(ArchivedProject project, ArchivedProjectVersion version, FileSet fileSet)
    {
      _project = project;
      _version = version;
      _fileSet = fileSet;
    }
  }
}