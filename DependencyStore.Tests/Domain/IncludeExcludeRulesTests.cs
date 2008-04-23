using System;
using System.Collections.Generic;

using Machine.Testing;

using NUnit.Framework;

namespace DependencyStore.Domain
{
  [TestFixture]
  public class IncludeExcludeRulesTests : TestsFor<IncludeExcludeRules>
  {
    private IncludeExcludeRules _target;

    public override void BeforeEachTest()
    {
      base.BeforeEachTest();
      _target = new IncludeExcludeRules();
    }
  }
}
