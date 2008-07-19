using System;
using System.Collections.Generic;

using DependencyStore.Domain;
using DependencyStore.Domain.Archiving;
using DependencyStore.Domain.Configuration;
using DependencyStore.Services.DataAccess;

using Machine.Core.Utility;
using Machine.Core.Services;

namespace DependencyStore.Services.Impl
{
  public class Controller : IController
  {
    private readonly IFileAndDirectoryRulesRepository _fileAndDirectoryRulesRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly ILocationRepository _locationRepository;
    private readonly IFileSystem _fileSystem;

    public Controller(ILocationRepository locationRepository, IFileAndDirectoryRulesRepository fileAndDirectoryRulesRepository, IProjectRepository projectRepository, IFileSystem fileSystem)
    {
      _projectRepository = projectRepository;
      _fileAndDirectoryRulesRepository = fileAndDirectoryRulesRepository;
      _locationRepository = locationRepository;
      _fileSystem = fileSystem;
    }

    #region IController Members
    public void Show(DependencyStoreConfiguration configuration)
    {
      DomainEvents.EncounteredOutdatedSinkFile += ReportOutdatedFile;
      DomainEvents.LocationNotFound += LocationNotFound;
      DomainEvents.Progress += Progress;
      CheckForNewerFiles(configuration);
    }

    public void Update(DependencyStoreConfiguration configuration)
    {
      DomainEvents.EncounteredOutdatedSinkFile += UpdateOutdatedFile;
      DomainEvents.LocationNotFound += LocationNotFound;
      DomainEvents.Progress += Progress;
      CheckForNewerFiles(configuration);
    }

    public void ArchiveProjects(DependencyStoreConfiguration configuration)
    {
      DomainEvents.Progress += Progress;
      BuildProjectArchives(configuration);
    }
    #endregion

    private void CheckForNewerFiles(DependencyStoreConfiguration configuration)
    {
      FileAndDirectoryRules rules = _fileAndDirectoryRulesRepository.FindDefault();
      IList<SourceLocation> sources = _locationRepository.FindAllSources(configuration, rules);
      IList<SinkLocation> sinks = _locationRepository.FindAllSinks(configuration, rules);
      IList<Project> projects = _projectRepository.FindAllProjects(configuration, rules);
      LatestFileSet latestFiles = new LatestFileSet();
      latestFiles.AddAll(sources);
      
      foreach (Project project in projects)
      {
        FileSet fileSet = project.Location.ToFileSet();
        FileSystemPath fileRootDirectory = fileSet.FindCommonDirectory();
        Archive archive = new Archive();
        foreach (FileSystemFile file in fileSet.Files)
        {
          archive.Add(file, file.Path.Chroot(fileRootDirectory));
        }
        archive.WriteZip(configuration.PackageDirectory.Join(project.Name + ".zip"));
      }

      foreach (SinkLocation location in sinks)
      {
        Console.WriteLine("Under {0}", location.Path.AsString);
        location.CheckForNewerFiles(latestFiles);
      }
    }

    private void BuildProjectArchives(DependencyStoreConfiguration configuration)
    {
      FileAndDirectoryRules rules = _fileAndDirectoryRulesRepository.FindDefault();
      IList<Project> projects = _projectRepository.FindAllProjects(configuration, rules);
      
      foreach (Project project in projects)
      {
        Archive archive = project.MakeArchive();
        archive.WriteZip(configuration.PackageDirectory.Join(project.Name + Archive.ZipExtension));
      }
    }

    private static void ReportOutdatedFile(object sender, OutdatedSinkFileEventArgs e)
    {
      TimeSpan age = e.SourceFile.ModifiedAt - e.SinkFile.ModifiedAt;
      FileSystemPath chrooted = e.SinkFile.Path.Chroot(e.SinkLocation.Path);
      Console.WriteLine("  {0} ({1} old)", chrooted.AsString, TimeSpanHelper.ToPrettyString(age));
    }

    private void UpdateOutdatedFile(object sender, OutdatedSinkFileEventArgs e)
    {
      try
      {
        ReportOutdatedFile(sender, e);
        _fileSystem.CopyFile(e.SourceFile.Path.AsString, e.SinkFile.Path.AsString, true);
      }
      catch (Exception error)
      {
        Console.WriteLine("Error copying {0}: {1}", e.SinkFile.Path.AsString, error.Message);
      }
    }

    private static void LocationNotFound(object sender, LocationNotFoundEventArgs e)
    {
      Console.WriteLine("Missing Location: {0}", e.Path);
    }
    
    private static void Progress(object sender, ProgressEventArgs e)
    {
      Console.Write("Archiving: {0}%\r", e.PercentComplete * 100);
    }
  }
}
