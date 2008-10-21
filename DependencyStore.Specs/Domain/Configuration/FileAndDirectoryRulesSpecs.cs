using System;
using System.Collections.Generic;

using Machine.Specifications;

using NUnit.Framework;

namespace DependencyStore.Domain.Configuration
{
  [Subject("Tests that became specs")]
  public class FileAndDirectoryRulesSpecs
  {
    static FileAndDirectoryRules _target;

    Establish context = () =>
    {
      _target = new FileAndDirectoryRules();
    };

    It Ctor_always_creates_file_rules_with_default_exclude = () =>
    {
      Assert.AreEqual(IncludeExclude.Exclude, _target.FileRules.Default);
    };

    It Ctor_always_creates_directory_rules_with_default_include = () =>
    {
      Assert.AreEqual(IncludeExclude.Include, _target.DirectoryRules. Default);
    };
  }
}
