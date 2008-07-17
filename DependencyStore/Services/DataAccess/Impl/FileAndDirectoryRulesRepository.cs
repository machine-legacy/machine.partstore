using System;
using System.Collections.Generic;

using DependencyStore.Domain;

namespace DependencyStore.Services.DataAccess.Impl
{
  public class FileAndDirectoryRulesRepository : IFileAndDirectoryRulesRepository
  {
    #region IFileAndDirectoryRulesRepository Members
    public FileAndDirectoryRules FindDefault()
    {
      FileAndDirectoryRules rules = new FileAndDirectoryRules();
      rules.DirectoryRules.AddExclusion(DefaultInclusionRules.GitDirectory);
      rules.DirectoryRules.AddExclusion(DefaultInclusionRules.SubversionDirectory);
      rules.DirectoryRules.AddExclusion(DefaultInclusionRules.PtDirectory);
      rules.DirectoryRules.AddExclusion(DefaultInclusionRules.ObjDirectory);
      rules.FileRules.AddInclusion(DefaultInclusionRules.DllFile);
      rules.FileRules.AddInclusion(DefaultInclusionRules.PdbFile);
      rules.FileRules.AddInclusion(DefaultInclusionRules.ConfigFile);
      return rules;
    }
    #endregion
  }
}
