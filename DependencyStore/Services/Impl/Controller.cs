using System;
using System.Collections.Generic;

using DependencyStore.Domain;
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
      CheckForNewerFiles(configuration);
    }

    public void Update(DependencyStoreConfiguration configuration)
    {
      DomainEvents.EncounteredOutdatedSinkFile += UpdateOutdatedFile;
      CheckForNewerFiles(configuration);
    }
    #endregion

    private void CheckForNewerFiles(DependencyStoreConfiguration configuration)
    {
      FileAndDirectoryRules rules = _fileAndDirectoryRulesRepository.FindDefault();
      IList<SourceLocation> sources = _locationRepository.FindAllSources(configuration, rules);
      IList<SinkLocation> sinks = _locationRepository.FindAllSinks(configuration, rules);
      LatestFiles latest = new LatestFiles();
      latest.AddAll(sources);
      foreach (SinkLocation location in sinks)
      {
        Console.WriteLine("Under {0}", location.Path.Full);
        location.CheckForNewerFiles(latest);
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
      ReportOutdatedFile(sender, e);
      _fileSystem.CopyFile(e.SourceFile.Path.Full, e.SinkFile.Path.Full, true);
    }
  }
}
