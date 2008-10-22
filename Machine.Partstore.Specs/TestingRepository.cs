using System;
using System.IO;

using Machine.Partstore.Commands;
using Machine.Partstore.Domain.Configuration;
using Machine.Container;

namespace Machine.Partstore
{
  public class TestingRepository : DirectoryManipulator
  {
    public TestingRepository()
      : base(Path.Combine(ConfigurationPaths.RootDataDirectory, @"TestRepository"))
    {
    }

    public Int32 NumberOfChildDirectories
    {
      get { return Directory.GetDirectories(this.RootDirectory).Length; }
    }

    public void AddVersion()
    {
      AddNewVersionCommand command = IoC.Container.Resolve.Object<AddNewVersionCommand>();
      command.Run();
    }
  }
}