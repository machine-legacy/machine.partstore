using System;
using System.Collections.Generic;

using Machine.Testing;

using NUnit.Framework;

namespace DependencyStore.Domain.FileSystem
{
  [TestFixture]
  public class FileSystemDirectorySpecs : TestsFor<FileSystemDirectory>
  {
    private FileSystemFile _aFile;
    private FileSystemDirectory _target;
    private FileSystemDirectory _aDirectoryWithFiles;

    [Test]
    public void GetName_always_is_just_name()
    {
      Assert.AreEqual(@"Windows", _target.Name);
    }

    [Test]
    public void GetEntries_none_is_empty()
    {
      CollectionAssert.IsEmpty(_target.Entries);
    }

    [Test]
    public void GetEntries_after_adding_a_directory_with_files_has_that_entry_and_not_its_children()
    {
      _target.Entries.Add(_aDirectoryWithFiles);
      CollectionAssert.AreEquivalent(new FileSystemEntry[] { _aDirectoryWithFiles }, _target.Entries);
    }

    [Test]
    public void GetEntries_after_adding_has_that_entry()
    {
      _target.Entries.Add(_aFile);
      CollectionAssert.AreEquivalent(new FileSystemEntry[] { _aFile }, _target.Entries);
    }

    [Test]
    public void GetEntries_after_adding_and_removing_is_empty()
    {
      _target.Entries.Add(_aFile);
      _target.Entries.Remove(_aFile);
      CollectionAssert.IsEmpty(_target.Entries);
    }

    [Test]
    public void GetBreadthFirstFiles_has_just_a_child_has_that_child()
    {
      _target.Entries.Add(_aFile);
      CollectionAssert.AreEquivalent(new FileSystemEntry[] { _aFile }, _target.Entries);
    }

    [Test]
    public void GetBreadthFirstFiles_has_a_directory_with_children_has_those_child()
    {
      _target.Entries.Add(_aDirectoryWithFiles);
      CollectionAssert.AreEquivalent(new FileSystemEntry[] { _aDirectoryWithFiles.Entries[0] }, new List<FileSystemFile>(_target.BreadthFirstFiles));
    }

    [Test]
    public void GetBreadthFirstFiles_has_a_child_and_a_directory_with_children_has_those_child()
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
