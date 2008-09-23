using System;
using System.Collections.Generic;
using System.Diagnostics;

using DependencyStore.Domain.Core;
using DependencyStore.Utility;

namespace DependencyStore.Domain.Distribution
{
  public class Hooks
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(Hooks));
    private readonly Purl _path;

    protected Hooks(Purl path)
    {
      _path = path;
    }

    public static Hooks Create(Repository repository)
    {
      return new Hooks(repository.RootPath.Join("Hooks"));
    }

    public void RunCommit(ArchivedProject project, ArchivedProjectVersion version)
    {
      Run(FindRunnableHooks("post-commit"), project.Name, version.CreatedAtVersion, version.RepositoryAlias);
    }

    public void RunRefresh()
    {
      Run(FindRunnableHooks("refresh"));
    }

    private IEnumerable<RunnableHook> FindRunnableHooks(string name)
    {
      if (Infrastructure.FileSystem.IsDirectory(_path.AsString))
      {
        foreach (string file in Infrastructure.FileSystem.GetFiles(_path.AsString, name + ".*"))
        {
          _log.Info("Found: " + file);
          yield return new RunnableHook(new Purl(file));
        }
      }
    }

    public void Run(IEnumerable<RunnableHook> hooks, params string[] parameters)
    {
      foreach (RunnableHook hook in hooks)
      {
        hook.Run(parameters);
      }
    }
  }
  public class RunnableHook
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(RunnableHook));
    private readonly Purl _path;

    public RunnableHook(Purl path)
    {
      _path = path;
    }

    public void Run(string[] parameters)
    {
      _log.Info("Running " + _path.AsString + " with " + parameters.Join(" "));
      ProcessStartInfo startInfo = new ProcessStartInfo(_path.AsString, parameters.Join(" "));
      startInfo.UseShellExecute = true;
      Process process = Process.Start(startInfo);
      if (process == null)
      {
        throw new InvalidOperationException("Error executing hook: " + _path);
      }
      process.WaitForExit();
    }
  }
}
