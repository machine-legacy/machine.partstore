using System;
using System.Collections.Generic;

using DependencyStore.Domain;

namespace DependencyStore.Services.DataAccess
{
  public class FileAndDirectoryRulesRepository : IFileAndDirectoryRulesRepository
  {
    #region IFileAndDirectoryRulesRepository Members
    public FileAndDirectoryRules FindDefault()
    {
      FileAndDirectoryRules rules = new FileAndDirectoryRules();
      rules.DirectoryRules.AddExclusion(@"^.svn$");
      rules.DirectoryRules.AddExclusion(@"^pt$");
      rules.DirectoryRules.AddExclusion(@"^obj$");
      rules.FileRules.AddInclusion(@"^.*\.dll$");
      rules.FileRules.AddInclusion(@"^.*\.pdb$");
      rules.FileRules.AddInclusion(@"^.*\.config$");
      return rules;
    }
    #endregion
  }
}
