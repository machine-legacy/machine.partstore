using System;
using System.Collections.Generic;

using Machine.Partstore.Application;
using Machine.Partstore.Utility;
using Machine.Partstore.Domain.Core;

namespace Machine.Partstore.Commands
{
  public class ShowCommand : Command
  {
    private readonly IAmForProjectState _projectState;

    public ShowCommand(IAmForProjectState projectState)
    {
      _projectState = projectState;
    }

    public override CommandStatus Run()
    {
      CurrentProjectState state = _projectState.GetCurrentProjectState();
      if (state.MissingConfiguration)
      {
        Console.WriteLine("Unable to find configuration!");
        return CommandStatus.Failure;
      }
      Console.WriteLine("Current Project: {0}", state.ProjectName);
      Console.WriteLine("References:");
      CommandStatus commandStatus = CommandStatus.Success;
      foreach (ReferenceStatus status in state.References)
      {
        if (!WriteStatus(status))
        {
          commandStatus = CommandStatus.Failure;
        }
      }
      if (state.References.Count == 0)
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
      if (status.IsHealthy)
      {
        if (status.IsOutdated)
        {
          flags.Add("Outdated");
        }
        if (!status.IsReferencedVersionInstalled)
        {
          flags.Add("NeedsUnpackage");
        }
        if (!status.IsAnyVersionInstalled)
        {
          flags.Add("NothingInstalled");
        }
        else if (status.IsOlderVersionInstalled)
        {
          flags.Add("OlderVersionInstalled");
        }
      }
      List<string> parts = new List<string>();
      parts.Add("  " + status.DependencyName);
      if (!status.ReferencedVersionTags.Empty)
      {
        parts.Add("(" + status.ReferencedVersionTags + ")");
      }
      if (flags.Count > 0)
      {
        parts.Add("(" + flags.Join(", ") + ")");
      }
      Console.WriteLine(parts.ToArray().Join(" "));
      return !status.IsHealthy;
    }
  }
}