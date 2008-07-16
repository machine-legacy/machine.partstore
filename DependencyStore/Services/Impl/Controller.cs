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
    private readonly ILocationRepository _locationRepository;
    private readonly IFileSystem _fileSystem;

    public Controller(ILocationRepository locationRepository, IFileAndDirectoryRulesRepository fileAndDirectoryRulesRepository, IFileSystem fileSystem)
    {
      _fileSystem = fileSystem;
      _fileAndDirectoryRulesRepository = fileAndDirectoryRulesRepository;
      _locationRepository = locationRepository;
    }

    #region IController Members
    public void Show(DependencyStoreConfiguration configuration)
    {
      DomainEvents.EncounteredOutdatedSinkFile += ReportOutdatedFile;
      DomainEvents.LocationNotFound += LocationNotFound;
      CheckForNewerFiles(configuration);
    }

    public void Update(DependencyStoreConfiguration configuration)
    {
      DomainEvents.EncounteredOutdatedSinkFile += UpdateOutdatedFile;
      DomainEvents.LocationNotFound += LocationNotFound;
      CheckForNewerFiles(configuration);
    }
    #endregion

    private void CheckForNewerFiles(DependencyStoreConfiguration configuration)
    {
      FileAndDirectoryRules rules = _fileAndDirectoryRulesRepository.FindDefault();
      IList<SourceLocation> sources = _locationRepository.FindAllSources(configuration, rules);
      IList<SinkLocation> sinks = _locationRepository.FindAllSinks(configuration, rules);
      LatestFileSet latestFiles = new LatestFileSet();
      latestFiles.AddAll(sources);
      /*
      foreach (SourceLocation location in sources)
      {
        FileSet fileSet = location.ToFileSet();
        FileSystemPath fileRootDirectory = fileSet.FindCommonDirectory();
        Archive archive = new Archive();
        Console.WriteLine("{0} {1}", location, fileRootDirectory);
        foreach (FileSystemFile file in fileSet.Files)
        {
          archive.Add(file, file.Path.Chroot(fileRootDirectory));
        }
        archive.WriteZip(location.Path.Join("Test.zip"));
      }
      */
      foreach (SinkLocation location in sinks)
      {
        Console.WriteLine("Under {0}", location.Path.Full);
        location.CheckForNewerFiles(latestFiles);
      }
    }

    private static void ReportOutdatedFile(object sender, OutdatedSinkFileEventArgs e)
    {
      TimeSpan age = e.SourceFile.ModifiedAt - e.SinkFile.ModifiedAt;
      FileSystemPath chrooted = e.SinkFile.Path.Chroot(e.SinkLocation.Path);
      Console.WriteLine("  {0} ({1} old)", chrooted.Full, TimeSpanHelper.ToPrettyString(age));
    }

    private void UpdateOutdatedFile(object sender, OutdatedSinkFileEventArgs e)
    {
      try
      {
        ReportOutdatedFile(sender, e);
        _fileSystem.CopyFile(e.SourceFile.Path.Full, e.SinkFile.Path.Full, true);
      }
      catch (Exception error)
      {
        Console.WriteLine("Error copying {0}: {1}", e.SinkFile.Path.Full, error.Message);
      }
    }

    private static void LocationNotFound(object sender, LocationNotFoundEventArgs e)
    {
      Console.WriteLine("Missing Location: {0}", e.Path);
    }
  }
}
