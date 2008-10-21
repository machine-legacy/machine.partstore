using System;
using System.IO;

using DependencyStore.Domain.Configuration;

namespace DependencyStore
{
  public class TestingRepository : DirectoryManipulator
  {
    public TestingRepository()
      : base(Path.Combine(ConfigurationPaths.RootDataDirectory, @"TestRepository"))
    {
    }
  }
}