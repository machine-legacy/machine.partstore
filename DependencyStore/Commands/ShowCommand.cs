using System;
using System.Collections.Generic;

using DependencyStore.Utility;
using DependencyStore.Domain.Distribution;
using DependencyStore.Domain.Distribution.Repositories;

namespace DependencyStore.Commands
{
  public class ShowCommand : Command
  {
    private readonly ICurrentProjectRepository _currentProjectRepository;

    public ShowCommand(ICurrentProjectRepository currentProjectRepository)
    {
      _currentProjectRepository = currentProjectRepository;
    }

    public override CommandStatus Run()
    {
      CurrentProject project = _currentProjectRepository.FindCurrentProject();
      Console.WriteLine("Current Project: {0}", project.Name);
      Console.WriteLine("References:");
      List<ReferenceStatus> referenceStatuses = new List<ReferenceStatus>(project.ReferenceStatuses);
      CommandStatus commandStatus = CommandStatus.Success;
      foreach (ReferenceStatus status in referenceStatuses)
      {
        if (!WriteStatus(status))
        {
          commandStatus = CommandStatus.Failure;
        }
      }
      if (referenceStatuses.Count == 0)
      {
        Console.WriteLine("No references, use 'ds add <project>' to add some.");
      }
      return commandStatus;
    }

    private static bool WriteStatus(ReferenceStatus status)
    {
      List<string> flags = new List<string>();
      if (status.IsProjectMissing)
      {
        flags.Add("Missing Project");
      }
      if (status.IsReferencedVersionMissing)
      {
        flags.Add("Missing Version");
      }
      if (status.IsOutdated)
      {
        flags.Add("Outdated");
      }
      if (status.IsReferencedVersionInstalled)
      {
        flags.Add("NeedsUpdate");
      }
      if (!status.IsAnyVersionInstalled)
      {
        flags.Add("NothingInstalled");
      }
      else if (status.IsOlderVersionInstalled)
      {
        flags.Add("OlderVersionInstalled");
      }
      Console.WriteLine("  {0} ({1}) ({2})", status.DependencyName, status.ReferencedVersionCreatedAt, flags.Join(", "));
      return !status.IsHealthy;
    }
  }
}