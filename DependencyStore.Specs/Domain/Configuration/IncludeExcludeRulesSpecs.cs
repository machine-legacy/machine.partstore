using System;
using System.Collections.Generic;

using DependencyStore.Domain.FileSystem;

using Machine.Testing;
using NUnit.Framework;

namespace DependencyStore.Domain.Configuration
{
  [TestFixture]
  public class IncludeExcludeRulesSpecs : TestsFor<IncludeExcludeRules>
  {
    private IncludeExcludeRules _target;
    private readonly Purl _simpleFileA = new Purl(@"C:\Windows\System32\Notepad.exe");
    private readonly Purl _simpleFileB = new Purl(@"C:\Windows\System32\Notepad.dll");
    private readonly Purl _simpleFileC = new Purl(@"C:\Windows\System32\Notepad.pdb");
    private readonly Purl _subversionDir = new Purl(@"C:\Home\Source\.svn");
    private readonly Purl _mightLookLikeSubversionDir = new Purl(@"C:\Home\Source\Asvn");

    public override void BeforeEachTest()
    {
      base.BeforeEachTest();
      _target = new IncludeExcludeRules();
    }

    [Test]
    public void Includes_no_rules_is_default()
    {
      Assert.AreEqual(_target.Default, _target.Includes(_simpleFileA));
    }

    [Test]
    public void Includes_rule_that_excludes_svn_and_is_svn_is_excluded()
    {
      _target.AddExclusion(DefaultInclusionRules.SubversionDirectory);
      Assert.AreEqual(IncludeExclude.Exclude, _target.Includes(_subversionDir));
    }

    [Test]
    public void Includes_rule_that_excludes_svn_and_looks_sike_svn_is_unknown()
    {
      _target.AddExclusion(DefaultInclusionRules.SubversionDirectory);
      Assert.AreEqual(IncludeExclude.Unknown, _target.Includes(_mightLookLikeSubversionDir));
    }

    [Test]
    public void Includes_has_rule_doesnt_match_is_default()
    {
      _target.AddExclusion(@"^$");
      Assert.AreEqual(IncludeExclude.Unknown, _target.Includes(_simpleFileA));
    }

    [Test]
    public void Includes_has_multiple_rules_that_dont_match_is_default()
    {
      _target.AddInclusion(DefaultInclusionRules.DllFile);
      _target.AddInclusion(DefaultInclusionRules.PdbFile);
      Assert.AreEqual(IncludeExclude.Unknown, _target.Includes(_simpleFileA));
    }

    [Test]
    public void Includes_Has_An_Inclusion_As_Match_Is_Include()
    {
      _target.AddInclusion(DefaultInclusionRules.DllFile);
      Assert.AreEqual(IncludeExclude.Include, _target.Includes(_simpleFileB));
    }

    [Test]
    public void Includes_has_multiple_an_inclusion_is_match_is_include()
    {
      _target.AddInclusion(DefaultInclusionRules.DllFile);
      _target.AddInclusion(DefaultInclusionRules.PdbFile);
      Assert.AreEqual(IncludeExclude.Include, _target.Includes(_simpleFileC));
    }
  }
}
