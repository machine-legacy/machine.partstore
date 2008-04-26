using System;
using System.Collections.Generic;

using NUnit.Framework;

namespace DependencyStore.Domain
{
  [TestFixture]
  public class BuildLocationTests
  {
    [Test]
    public void GetIsSink_Always_IsFalse()
    {
      SourceLocation location = new SourceLocation();
      Assert.IsFalse(location.IsSink);
    }

    [Test]
    public void GetIsSource_Always_IsTrue()
    {
      SourceLocation location = new SourceLocation();
      Assert.IsTrue(location.IsSource);
    }
  }
}