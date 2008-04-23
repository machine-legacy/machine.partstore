using System;
using System.Collections.Generic;

namespace DependencyStore.Domain
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

    public IncludeExclude IncludesFile(FileSystemPath path)
    {
      return _fileRules.Includes(path);
    }

    public IncludeExclude IncludesDirectory(FileSystemPath path)
    {
      return _directoryRules.Includes(path);
    }
  }
}