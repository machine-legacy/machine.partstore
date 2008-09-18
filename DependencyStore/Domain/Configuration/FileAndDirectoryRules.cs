using System;
using System.Collections.Generic;

using DependencyStore.Domain.Core;

namespace DependencyStore.Domain.Configuration
{
  public class FileAndDirectoryRules
  {
    private readonly IncludeExcludeRules _fileRules = new IncludeExcludeRules(IncludeExclude.Exclude);
    private readonly IncludeExcludeRules _directoryRules = new IncludeExcludeRules(IncludeExclude.Include);

    public IncludeExcludeRules FileRules
    {
      get { return _fileRules; }
    }

    public IncludeExcludeRules DirectoryRules
    {
      get { return _directoryRules; }
    }

    public IncludeExclude IncludesFile(Purl path)
    {
      return _fileRules.Includes(path);
    }

    public IncludeExclude IncludesDirectory(Purl path)
    {
      return _directoryRules.Includes(path);
    }
  }
}