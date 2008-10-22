using System;
using System.Collections.Generic;

using Machine.Specifications;

using NUnit.Framework;

namespace Machine.Partstore.Domain.FileSystem
{
  public class with_new_directory
  {
    protected static FileSystemFile _aFile;
    protected static FileSystemDirectory _target;
    protected static FileSystemDirectory _aDirectoryWithFiles;

    Establish context = () =>
    {
      _target = new FileSystemDirectory(new Purl(@"C:\Windows"));
      _aFile = new FileSystemFile(new Purl(@"C:\Boot.ini"), 0, DateTime.Now, DateTime.Now, DateTime.Now);
      _aDirectoryWithFiles = new FileSystemDirectory(new Purl(@"C:\Windows\System32"));
      _aDirectoryWithFiles.Entries.Add(new FileSystemFile(new Purl(@"C:\Windows\System32\Notepad.exe"), 0, DateTime.Now, DateTime.Now, DateTime.Now));
    };
  }

  [Subject("Tests that became specs")]
  public class when_working_with_directory : with_new_directory
  {
    It should_have_expected_name = () =>
    {
      Assert.AreEqual(@"Windows", _target.Name);
    };

    It should_be_empty = () =>
    {
      CollectionAssert.IsEmpty(_target.Entries);
    };
  }

  [Subject("Tests that became specs")]
  public class when_adding_directory_with_children_to_directory : with_new_directory
  {
    Establish context = () =>
    {
      _target.Entries.Add(_aDirectoryWithFiles);
    };

    It should_be_in_entries = () =>
    {
      CollectionAssert.AreEquivalent(new FileSystemEntry[] { _aDirectoryWithFiles }, _target.Entries);
    };

    It should_have_expected_breadth_first_traversal = () =>
    {
      CollectionAssert.AreEquivalent(new FileSystemEntry[] { _aDirectoryWithFiles.Entries[0] }, new List<FileSystemFile>(_target.BreadthFirstFiles));
    };
  }

  [Subject("Tests that became specs")]
  public class when_adding_file_to_directory : with_new_directory
  {
    Establish context = () =>
    {
      _target.Entries.Add(_aFile);
    };

    It should_be_in_entries = () =>
    {
      CollectionAssert.AreEquivalent(new FileSystemEntry[] { _aFile }, _target.Entries);
    };

    It should_have_expected_breadth_first_traversal = () =>
    {
      CollectionAssert.AreEquivalent(new FileSystemEntry[] { _aFile }, _target.Entries);
    };
  }

  [Subject("Tests that became specs")]
  public class when_adding_and_removing_file_from_directory : with_new_directory
  {
    Establish context = () =>
    {
      _target.Entries.Add(_aFile);
      _target.Entries.Remove(_aFile);
    };

    It should_be_empty = () =>
    {
      CollectionAssert.IsEmpty(_target.Entries);
    };
  }

  [Subject("Tests that became specs")]
  public class when_adding_file_and_directory_to_directory : with_new_directory
  {
    Establish context = () =>
    {
      _target.Entries.Add(_aFile);
      _target.Entries.Add(_aDirectoryWithFiles);
    };

    It should_have_expected_breadth_first_traversal = () =>
    {
      CollectionAssert.AreEquivalent(new[] { _aFile, _aDirectoryWithFiles.Entries[0] }, new List<FileSystemFile>(_target.BreadthFirstFiles));
    };
  }
}
