using System;
using System.Collections.Generic;
using System.IO;

using Machine.Core.Services;

namespace DependencyStore.Domain.Configuration
{
  public class ConfigurationPaths
  {
    private readonly IFileSystem _fileSystem;
    private const string FileName = @"DependencyStore.config";

    public static string RootDataDirectory
    {
      get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "DependencyStore"); }
    }

    public ConfigurationPaths(IFileSystem fileSystem)
    {
      _fileSystem = fileSystem;
    }

    protected virtual string[] FindAllPossiblePaths()
    {
      List<string> paths = new List<string>();
      paths.Add(FileName);
      paths.Add(Path.Combine(RootDataDirectory, FileName));
      return paths.ToArray();
    }

    public string FindDefaultConfigurationPath()
    {
      foreach (string path in FindAllPossiblePaths())
      {
        if (_fileSystem.IsFile(path))
        {
          return path;
        }
      }
      return null;
    }

    public string FindConfigurationForCurrentProjectPath()
    {
      foreach (string directory in WalkUpDirectories())
      {
        string wouldBe = Path.Combine(directory, FileName);
        if (_fileSystem.IsFile(wouldBe))
        {
          return wouldBe;
        }
      }
      return null;
    }

    public string InferProjectRootDirectory()
    {
      foreach (string directory in WalkUpDirectories())
      {
        if (CouldBeProjectRootDirectory(directory))
        {
          return directory;
        }
      }
      return null;
    }

    public string InferPathToConfigurationForCurrentProject()
    {
      string path = InferProjectRootDirectory();
      if (String.IsNullOrEmpty(path))
      {
        return null;
      }
      return Path.Combine(path, FileName);
    }

    private static IEnumerable<string> WalkUpDirectories()
    {
      string directory = Environment.CurrentDirectory;
      while (directory != null)
      {
        yield return directory;
        directory = Path.GetDirectoryName(directory);
      }
    }

    private bool CouldBeProjectRootDirectory(string directory)
    {
      string[] indicators = new string[] { ".git", ".gitignore", "rakefile.rb", "DependencyStore.config" };
      foreach (string indicator in indicators)
      {
        string wouldBe = Path.Combine(directory, indicator);
        if (_fileSystem.IsDirectory(wouldBe) || _fileSystem.IsFile(wouldBe))
        {
          return true;
        }
      }
      return false;
    }
  }
}