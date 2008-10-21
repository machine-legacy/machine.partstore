using System;
using System.IO;

using DependencyStore.Commands;
using DependencyStore.Domain.Configuration;
using Machine.Container;

namespace DependencyStore
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