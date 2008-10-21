using System;
using System.Collections.Generic;

using Machine.Testing;

using NUnit.Framework;

namespace DependencyStore.Domain.Configuration
{
  [TestFixture]
  public class FileAndDirectoryRulesSpecs : TestsFor<FileAndDirectoryRules>
  {
    private FileAndDirectoryRules _target;

    [Test]
    public void Ctor_always_creates_file_rules_with_default_exclude()
    {
      Assert.AreEqual(IncludeExclude.Exclude, _target.FileRules.Default);
    }

    [Test]
    public void Ctor_always_creates_directory_rules_with_default_include()
    {
      Assert.AreEqual(IncludeExclude.Include, _target.DirectoryRules.Default);
    }

    public override void BeforeEachTest()
    {
      base.BeforeEachTest();
      _target = new FileAndDirectoryRules();
    }
  }
}
