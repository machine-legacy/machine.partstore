using System;
using System.Collections.Generic;

using Machine.Testing;

using NUnit.Framework;

namespace DependencyStore.Services.DataAccess.Impl
{
  [TestFixture]
  public class FileAndDirectoryRulesRepositoryTests : TestsFor<FileAndDirectoryRulesRepository>
  {
    [Test]
    public void FindDefault_Always_IsRules()
    {
      Assert.IsNotNull(this.Target.FindDefault());
    }
  }
}
