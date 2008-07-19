using System;
using System.Collections.Generic;

using Machine.Testing;

using NUnit.Framework;

namespace DependencyStore.Domain
{
  [TestFixture]
  public class FileSystemPathTests : TestsFor<Purl>
  {
    private Purl _target;

    [Test]
    public void GetFull_Always_FullPath()
    {
      Assert.AreEqual(@"C:\WINDOWS\SYSTEM32\Notepad.exe", _target.AsString);
    }

    [Test]
    public void GetName_Always_JustName()
    {
      Assert.AreEqual(@"Notepad.exe", _target.Name);
    }

    [Test]
    public void GetDirectory_Always_JustDirectory()
    {
      Assert.AreEqual(@"C:\WINDOWS\SYSTEM32", _target.Directory);
    }

    [Test]
    public void Equals_SamePath_IsTrue()
    {
      Purl other = new Purl(_target.AsString);
      Assert.IsTrue(_target.Equals(other));
    }

    [Test]
    public void Equals_DifferentPath_IsFalse()
    {
      Purl other = new Purl(@"C:\WINDOWS");
      Assert.IsFalse(_target.Equals(other));
    }

    [Test]
    public void Equals_SamePathDifferentCase_IsTrue()
    {
      Purl other = new Purl(_target.AsString.ToLower());
      Assert.IsTrue(_target.Equals(other));
    }

    [Test]
    public void Equals_SamePathWithVaryingIndirection_IsFalse()
    {
      Purl a = new Purl(@"C:\WINDOWS\SYSTEM32");
      Purl b = new Purl(@"C:\WINDOWS\..\WINDOWS\SYSTEM32");
      Assert.IsFalse(a.Equals(b));
    }

    public override void BeforeEachTest()
    {
      base.BeforeEachTest();
      _target = new Purl(@"C:\WINDOWS\SYSTEM32\Notepad.exe");
    }
  }
}
