using System;
using System.Collections.Generic;

using NUnit.Framework;

namespace DependencyStore.Domain
{
  [TestFixture]
  public class LibraryLocationTests
  {
    [Test]
    public void GetIsSink_Always_IsTrue()
    {
      LibraryLocation location = new LibraryLocation();
      Assert.IsTrue(location.IsSink);
    }

    [Test]
    public void GetIsSource_Always_IsFalse()
    {
      LibraryLocation location = new LibraryLocation();
      Assert.IsFalse(location.IsSource);
    }
  }
}
