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
      BuildLocation location = new BuildLocation();
      Assert.IsFalse(location.IsSink);
    }

    [Test]
    public void GetIsSource_Always_IsTrue()
    {
      BuildLocation location = new BuildLocation();
      Assert.IsTrue(location.IsSource);
    }
  }
}