using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

using Machine.Core.Services;
using Machine.Core.Utility;

using DependencyStore.Domain.Core;
using DependencyStore.Domain.Core.Repositories;

namespace DependencyStore.Domain.Configuration.Repositories.Impl
{
  public class ConfigurationRepository : IConfigurationRepository
  {
    private readonly IFileSystem _fileSystem;
    private readonly IFileAndDirectoryRulesRepository _fileAndDirectoryRulesRepository;
    private readonly ConfigurationPaths _paths;

    public ConfigurationRepository(IFileSystem fileSystem, IFileAndDirectoryRulesRepository fileAndDirectoryRulesRepository, ConfigurationPaths paths)
    {
      _fileSystem = fileSystem;
      _fileAndDirectoryRulesRepository = fileAndDirectoryRulesRepository;
      _paths = paths;
    }

    #region IConfigurationRepository Members
    public DependencyStoreConfiguration FindConfiguration(string configurationFile)
    {
      try
      {
        if (configurationFile == null)
        {
          throw new FileNotFoundException();
        }
        using (StreamReader reader = _fileSystem.OpenText(configurationFile))
        {
          DependencyStoreConfiguration configuration = XmlSerializationHelper.DeserializeString<DependencyStoreConfiguration>(reader.ReadToEnd());
          configuration.EnsureValid();
          configuration.FileAndDirectoryRules = _fileAndDirectoryRulesRepository.FindDefault();
          return configuration;
        }
      }
      catch (InvalidOperationException e)
      {
        throw new InvalidConfigurationException("Error reading configuration", e);
      }
      catch (XmlException e)
      {
        throw new InvalidConfigurationException("Error reading configuration", e);
      }
    }

    public DependencyStoreConfiguration FindDefaultConfiguration()
    {
      return FindConfiguration(_paths.FindDefaultConfigurationPath());
    }

    public DependencyStoreConfiguration FindProjectConfiguration()
    {
      string path = _paths.FindConfigurationForCurrentProjectPath();
      DependencyStoreConfiguration configuration = FindConfiguration(path);
      if (configuration.ProjectConfigurations.Count == 0)
      {
        ProjectStructure projectStructure = new ProjectStructure(new Purl(Path.GetDirectoryName(path)));
        ProjectConfiguration currentProject = new ProjectConfiguration();
        currentProject.Root = new RootDirectoryConfiguration(projectStructure.FindRootDirectory());
        currentProject.Build = new BuildDirectoryConfiguration(projectStructure.FindBuildDirectory());
        currentProject.Library = new LibraryDirectoryConfiguration(projectStructure.FindLibraryDirectory());
        configuration.ProjectConfigurations.Add(currentProject);
      }
      return configuration;
    }
    #endregion
  }
  public class ProjectStructure
  {
    private readonly Purl _root;

    public ProjectStructure(Purl root)
    {
      _root = root;
    }

    public Purl FindRootDirectory()
    {
      return _root;
    }

    public Purl FindBuildDirectory()
    {
      return _root.Join("Build");
    }

    public Purl FindLibraryDirectory()
    {
      return _root.Join("Libraries");
    }
  }
}
