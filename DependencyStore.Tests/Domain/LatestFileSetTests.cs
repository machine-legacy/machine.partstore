using System;
using System.Collections.Generic;

using Machine.Testing;

using NUnit.Framework;

namespace DependencyStore.Domain
{
  [TestFixture]
  public class LatestFileSetTests : TestsFor<LatestFileSet>
  {
    private LatestFileSet _target;

    [Test]
    public void FindByExistingName_Nothing_IsNull()
    {
      FileSystemFile file = new FileSystemFile(new FileSystemPath(@"C:\File.txt"));
      Assert.IsNull(_target.FindExistingByName(file));
    }

    [Test]
    public void FindByExistingName_IsNotSameNameAsOneInThere_IsNull()
    {
      FileSystemFile file1 = new FileSystemFile(new FileSystemPath(@"C:\File1.txt"));
      FileSystemFile file2 = new FileSystemFile(new FileSystemPath(@"C:\File2.txt"));
      _target.Add(file1);
      Assert.IsNull(_target.FindExistingByName(file2));
    }

    [Test]
    public void FindByExistingName_IsSameNameAsOneInThere_IsFile()
    {
      FileSystemFile file1 = new FileSystemFile(new FileSystemPath(@"C:\File1.txt"));
      FileSystemFile file2 = new FileSystemFile(new FileSystemPath(@"C:\OtherPlace\File1.txt"));
      _target.Add(file1);
      Assert.AreEqual(file1, _target.FindExistingByName(file2));
    }

    [Test]
    public void FindByExistingName_IsSameNameAndPathAsOneInThere_IsFile()
    {
      FileSystemFile file1 = new FileSystemFile(new FileSystemPath(@"C:\File1.txt"));
      FileSystemFile file2 = new FileSystemFile(new FileSystemPath(@"C:\File1.txt"));
      _target.Add(file1);
      Assert.AreEqual(file1, _target.FindExistingByName(file2));
    }

    [Test]
    public void Add_NewFile_HasThatFile()
    {
      FileSystemFile file1 = new FileSystemFile(new FileSystemPath(@"C:\File1.txt"));
      _target.Add(file1);
      CollectionAssert.AreEqual(new FileSystemFile[] { file1 }, new List<FileSystemFile>(_target.Files));
    }

    [Test]
    public void Add_SecondFile_HasThoseFile()
    {
      FileSystemFile file1 = new FileSystemFile(new FileSystemPath(@"C:\File1.txt"));
      file1.ModifiedAt = DateTime.Now;
      FileSystemFile file2 = new FileSystemFile(new FileSystemPath(@"C:\File2.txt"));
      file2.ModifiedAt = DateTime.Now;
      _target.Add(file1);
      _target.Add(file2);
      CollectionAssert.AreEqual(new FileSystemFile[] { file1, file2 }, new List<FileSystemFile>(_target.Files));
    }

    [Test]
    public void Add_IsCollidingFileButSecondIsOlder_KeepsFirst()
    {
      FileSystemFile file1 = new FileSystemFile(new FileSystemPath(@"C:\File1.txt"));
      file1.ModifiedAt = DateTime.Now;
      FileSystemFile file2 = new FileSystemFile(new FileSystemPath(@"C:\File1.txt"));
      file2.ModifiedAt = file1.ModifiedAt - TimeSpan.FromDays(1.0);
      _target.Add(file1);
      _target.Add(file2);
      CollectionAssert.AreEqual(new FileSystemFile[] { file1 }, new List<FileSystemFile>(_target.Files));
    }

    [Test]
    public void Add_IsCollidingFileButSecondIsNewer_ReplacesFirst()
    {
      FileSystemFile file1 = new FileSystemFile(new FileSystemPath(@"C:\File1.txt"));
      file1.ModifiedAt = DateTime.Now;
      FileSystemFile file2 = new FileSystemFile(new FileSystemPath(@"C:\OtherPlace\File1.txt"));
      file2.ModifiedAt = file1.ModifiedAt + TimeSpan.FromDays(1.0);
      _target.Add(file1);
      _target.Add(file2);
      CollectionAssert.AreEqual(new FileSystemFile[] { file2 }, new List<FileSystemFile>(_target.Files));
    }

    public override void BeforeEachTest()
    {
      base.BeforeEachTest();
      _target = new LatestFileSet();
    }
  }
}
