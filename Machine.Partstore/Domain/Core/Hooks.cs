using System;
using System.Collections.Generic;
using System.Diagnostics;

using Machine.Partstore.Domain.FileSystem;
using Machine.Partstore.Utility;

namespace Machine.Partstore.Domain.Core
{
  public class HookType
  {
    private readonly string _fileExtension;
    private readonly Type _type;

    public string FileExtension
    {
      get { return _fileExtension; }
    }

    public Type Type
    {
      get { return _type; }
    }

    public HookType(string extension, Type type)
    {
      _fileExtension = extension;
      _type = type;
    }
  }
  public class Hooks
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(Hooks));
    private readonly Purl _path;
    private readonly List<HookType> _hookTypes = new List<HookType>();
    
    protected Hooks(Purl path)
    {
      _path = path;
      _hookTypes.Add(new HookType("cmd", typeof(CmdExecHook)));
      _hookTypes.Add(new HookType("ps1", typeof(PowershellHook)));
    }

    public static Hooks Create(Repository repository)
    {
      return new Hooks(repository.RootPath.Join("Hooks"));
    }

    public void RunCommit(ArchivedProject project, ArchivedProjectVersion version)
    {
      Run(FindRunnableHooks("post-commit"), _path.Parent.AsString, project.Name, version.Number.AsString, version.RepositoryAlias);
    }

    public void RunRefresh()
    {
      Run(FindRunnableHooks("refresh"), _path.Parent.AsString);
    }

    private IEnumerable<RunnableHook> FindRunnableHooks(string name)
    {
      if (Infrastructure.FileSystem.IsDirectory(_path.AsString))
      {
        foreach (HookType hookType in _hookTypes)
        {
          foreach (string file in Infrastructure.FileSystem.GetFiles(_path.AsString, name + "." + hookType.FileExtension))
          {
            _log.Info("Found: " + file);
            yield return (RunnableHook)Activator.CreateInstance(hookType.Type, new Purl(file));
          }
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

    protected Purl Path
    {
      get { return _path; }
    }

    public RunnableHook(Purl path)
    {
      _path = path;
    }

    public void Run(string[] parameters)
    {
      Purl repositoryDirectory = _path.Parent.Parent;
      string command = CreateCommand();
      string commandArguments = CreateArguments(parameters);
      _log.Info("Running " + command + " with " + commandArguments + " in " + repositoryDirectory.AsString);
      ProcessStartInfo startInfo = new ProcessStartInfo(command, commandArguments);
      startInfo.WorkingDirectory = repositoryDirectory.AsString;
      startInfo.RedirectStandardOutput = true;
      startInfo.RedirectStandardError = true;
      startInfo.UseShellExecute = false;
      Process process = Process.Start(startInfo);
      if (process == null)
      {
        throw new InvalidOperationException("Error executing hook: " + _path);
      }
      string standardOut = process.StandardOutput.ReadToEnd();
      if (!String.IsNullOrEmpty(standardOut))
      {
        Console.WriteLine(standardOut);
      }
      string standardError = process.StandardError.ReadToEnd();
      if (!String.IsNullOrEmpty(standardError))
      {
        Console.WriteLine(standardError);
      }
      process.WaitForExit();
    }

    protected virtual string CreateCommand()
    {
      return _path.AsString;
    }

    protected virtual string CreateArguments(string[] arguments)
    {
      return arguments.QuoteEach().Join(" ");
    }
  }
  public class CmdExecHook : RunnableHook
  {
    public CmdExecHook(Purl path)
      : base(path)
    {
    }
  }
  public class PowershellHook : RunnableHook
  {
    public PowershellHook(Purl path)
      : base(path)
    {
    }

    protected override string CreateCommand()
    {
      return @"C:\Windows\System32\WindowsPowershell\V1.0\Powershell.exe";
    }

    protected override string CreateArguments(string[] arguments)
    {
      return "-NoProfile &'" + base.CreateCommand() + "' " + arguments.QuoteEach("'").Join(" ");
    }
  }
}
