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

    public ConfigurationPaths(IFileSystem fileSystem)
    {
      _fileSystem = fileSystem;
    }

    protected virtual string[] FindAllPossiblePaths()
    {
      string applicationData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
      List<string> paths = new List<string>();
      paths.Add(FileName);
      paths.Add(Path.Combine(applicationData, FileName));
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
      string directory = Environment.CurrentDirectory;
      while (directory != null)
      {
        string wouldBe = Path.Combine(directory, FileName);
        if (_fileSystem.IsFile(wouldBe))
        {
          return wouldBe;
        }
        directory = Path.GetDirectoryName(directory);
      }
      return null;
    }
  }
}