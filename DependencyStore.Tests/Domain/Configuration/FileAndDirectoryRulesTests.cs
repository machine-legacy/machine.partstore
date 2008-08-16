using System;
using System.Collections.Generic;

using Machine.Testing;

using NUnit.Framework;

namespace DependencyStore.Domain.Configuration
{
  [TestFixture]
  public class FileAndDirectoryRulesTests : TestsFor<FileAndDirectoryRules>
  {
    private FileAndDirectoryRules _target;

    [Test]
    public void Ctor_Always_CreatesFileRulesWithDefaultExclude()
    {
      Assert.AreEqual(IncludeExclude.Exclude, _target.FileRules.Default);
    }

    [Test]
    public void Ctor_Always_CreatesDirectoryRulesWithDefaultInclude()
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
