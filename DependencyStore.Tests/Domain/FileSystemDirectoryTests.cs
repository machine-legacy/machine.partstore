using System;
using System.Collections.Generic;

using Machine.Testing;

using NUnit.Framework;

namespace DependencyStore.Domain
{
  [TestFixture]
  public class FileSystemDirectoryTests : TestsFor<FileSystemDirectory>
  {
    private FileSystemFile _aFile;
    private FileSystemDirectory _target;
    private FileSystemDirectory _aDirectoryWithFiles;

    [Test]
    public void GetName_Always_IsJustName()
    {
      Assert.AreEqual(@"Windows", _target.Name);
    }

    [Test]
    public void GetEntries_None_IsEmpty()
    {
      CollectionAssert.IsEmpty(_target.Entries);
    }

    [Test]
    public void GetEntries_AfterAddingADirectoryWithFiles_HasThatEntryAndNotItsChildren()
    {
      _target.Entries.Add(_aDirectoryWithFiles);
      CollectionAssert.AreEquivalent(new FileSystemEntry[] { _aDirectoryWithFiles }, _target.Entries);
    }

    [Test]
    public void GetEntries_AfterAdding_HasThatEntry()
    {
      _target.Entries.Add(_aFile);
      CollectionAssert.AreEquivalent(new FileSystemEntry[] { _aFile }, _target.Entries);
    }

    [Test]
    public void GetEntries_AfterAddingAndRemoving_IsEmpty()
    {
      _target.Entries.Add(_aFile);
      _target.Entries.Remove(_aFile);
      CollectionAssert.IsEmpty(_target.Entries);
    }

    [Test]
    public void GetBreadthFirstFiles_HasJustAChild_HasThatChild()
    {
      _target.Entries.Add(_aFile);
      CollectionAssert.AreEquivalent(new FileSystemEntry[] { _aFile }, _target.Entries);
    }

    [Test]
    public void GetBreadthFirstFiles_HasADirectoryWithChildren_HasThoseChild()
    {
      _target.Entries.Add(_aDirectoryWithFiles);
      CollectionAssert.AreEquivalent(new FileSystemEntry[] { _aDirectoryWithFiles.Entries[0] }, new List<FileSystemFile>(_target.BreadthFirstFiles));
    }

    [Test]
    public void GetBreadthFirstFiles_HasAChildAndADirectoryWithChildren_HasThoseChild()
    {
      _target.Entries.Add(_aFile);
      _target.Entries.Add(_aDirectoryWithFiles);
      CollectionAssert.AreEquivalent(new FileSystemEntry[] { _aFile, _aDirectoryWithFiles.Entries[0] }, new List<FileSystemFile>(_target.BreadthFirstFiles));
    }

    public override void BeforeEachTest()
    {
      base.BeforeEachTest();
      _target = new FileSystemDirectory(new Purl(@"C:\Windows"));
      _aFile = new FileSystemFile(new Purl(@"C:\Boot.ini"), 0, DateTime.Now, DateTime.Now, DateTime.Now);
      _aDirectoryWithFiles = new FileSystemDirectory(new Purl(@"C:\Windows\System32"));
      _aDirectoryWithFiles.Entries.Add(new FileSystemFile(new Purl(@"C:\Windows\System32\Notepad.exe"), 0, DateTime.Now, DateTime.Now, DateTime.Now));
    }
  }
}
