using System;
using System.Collections.Generic;

using Machine.Specifications;

using NUnit.Framework;

namespace Machine.Partstore.Domain.FileSystem
{
  [Subject("Tests that became specs")]
  public class FileSystemPathSpecs
  {
    static Purl _target;

    Establish context = () =>
    {
      _target = new Purl(@"C:\WINDOWS\SYSTEM32\Notepad.exe");
    };

    It GetFull_always_full_path = () =>
    {
      Assert.AreEqual(@"C:\WINDOWS\SYSTEM32\Notepad.exe", _target.AsString);
    };

    It GetName_always_just_name = () =>
    {
      Assert.AreEqual(@"Notepad.exe", _target.Name);
    };

    It GetDirectory_always_just_directory = () =>
    {
      Assert.AreEqual(@"C:\WINDOWS\SYSTEM32", _target.Directory);
    };

    It Equals_same_path_is_true = () =>
    {
      Purl other = new Purl(_target.AsString);
      Assert.IsTrue(_target.Equals(other));
    };

    It Equals_different_path_is_false = () =>
    {
      Purl other = new Purl(@"C:\WINDOWS");
      Assert.IsFalse(_target.Equals(other));
    };

    It Equals_same_path_different_case_is_true = () =>
    {
      Purl other = new Purl(_target.AsString.ToLower());
      Assert.IsTrue(_target.Equals(other));
    };

    It Equals_same_path_with_varying_indirection_is_false = () =>
    {
      Purl a = new Purl(@"C:\WINDOWS\SYSTEM32");
      Purl b = new Purl(@"C:\WINDOWS\..\WINDOWS\SYSTEM32");
      Assert.IsFalse(a.Equals(b));
    };
  }
}
