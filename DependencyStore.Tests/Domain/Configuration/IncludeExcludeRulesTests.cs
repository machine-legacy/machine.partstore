using System;
using System.Collections.Generic;

using DependencyStore.Domain.Core;

using Machine.Testing;
using NUnit.Framework;

namespace DependencyStore.Domain.Configuration
{
  [TestFixture]
  public class IncludeExcludeRulesTests : TestsFor<IncludeExcludeRules>
  {
    private IncludeExcludeRules _target;
    private Purl _simpleFileA = new Purl(@"C:\Windows\System32\Notepad.exe");
    private Purl _simpleFileB = new Purl(@"C:\Windows\System32\Notepad.dll");
    private Purl _simpleFileC = new Purl(@"C:\Windows\System32\Notepad.pdb");
    private Purl _subversionDir = new Purl(@"C:\Home\Source\.svn");
    private Purl _mightLookLikeSubversionDir = new Purl(@"C:\Home\Source\Asvn");

    public override void BeforeEachTest()
    {
      base.BeforeEachTest();
      _target = new IncludeExcludeRules();
    }

    [Test]
    public void Includes_NoRules_IsDefault()
    {
      Assert.AreEqual(_target.Default, _target.Includes(_simpleFileA));
    }

    [Test]
    public void Includes_RuleThatExcludesSvnAndIsSvn_IsExclude()
    {
      _target.AddExclusion(DefaultInclusionRules.SubversionDirectory);
      Assert.AreEqual(IncludeExclude.Exclude, _target.Includes(_subversionDir));
    }

    [Test]
    public void Includes_RuleThatExcludesSvnAndLooksLikeSvn_IsUnknown()
    {
      _target.AddExclusion(DefaultInclusionRules.SubversionDirectory);
      Assert.AreEqual(IncludeExclude.Unknown, _target.Includes(_mightLookLikeSubversionDir));
    }

    [Test]
    public void Includes_HasRuleDoesntMatch_IsDefault()
    {
      _target.AddExclusion(@"^$");
      Assert.AreEqual(IncludeExclude.Unknown, _target.Includes(_simpleFileA));
    }

    [Test]
    public void Includes_HasMultipleRulesThatDontMatch_IsDefault()
    {
      _target.AddInclusion(DefaultInclusionRules.DllFile);
      _target.AddInclusion(DefaultInclusionRules.PdbFile);
      Assert.AreEqual(IncludeExclude.Unknown, _target.Includes(_simpleFileA));
    }

    [Test]
    public void Includes_HasAnInclusionAsMatch_IsInclude()
    {
      _target.AddInclusion(DefaultInclusionRules.DllFile);
      Assert.AreEqual(IncludeExclude.Include, _target.Includes(_simpleFileB));
    }

    [Test]
    public void Includes_HasMultipleAnInclusionIsMatch_IsInclude()
    {
      _target.AddInclusion(DefaultInclusionRules.DllFile);
      _target.AddInclusion(DefaultInclusionRules.PdbFile);
      Assert.AreEqual(IncludeExclude.Include, _target.Includes(_simpleFileC));
    }
  }
}
