using System;
using System.Collections.Generic;

using NUnit.Framework;

namespace DependencyStore.Domain.Core
{
  [TestFixture]
  public class LibraryLocationTests
  {
    [Test]
    public void GetIsSink_Always_IsTrue()
    {
      SinkLocation location = new SinkLocation();
      Assert.IsTrue(location.IsSink);
    }

    [Test]
    public void GetIsSource_Always_IsFalse()
    {
      SinkLocation location = new SinkLocation();
      Assert.IsFalse(location.IsSource);
    }
  }
}
