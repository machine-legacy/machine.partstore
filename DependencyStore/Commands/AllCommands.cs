using System;
using System.Collections.Generic;

using DependencyStore.Domain.Configuration;

namespace DependencyStore.Commands
{
  public abstract class Command : ICommand
  {
    public abstract void Run(DependencyStoreConfiguration configuration);
  }
  public class ShowCommand : Command
  {
    public override void Run(DependencyStoreConfiguration configuration)
    {
    }
  }
  public class UnpackageCommand : Command
  {
    public override void Run(DependencyStoreConfiguration configuration)
    {
    }
  }
  public class AddDependencyCommand : Command
  {
    public override void Run(DependencyStoreConfiguration configuration)
    {
    }
  }
  public class AddNewVersionCommand : Command
  {
    public override void Run(DependencyStoreConfiguration configuration)
    {
    }
  }
  public class HelpCommand : Command
  {
    public override void Run(DependencyStoreConfiguration configuration)
    {
      Console.WriteLine("{0} <command> [options]", "DependencyStore.exe");
      Console.WriteLine("Commands:");
      Console.WriteLine("  show");
      Console.WriteLine("  unpackage --dry-run");
      Console.WriteLine("  add --dry-run");
      Console.WriteLine("  update --all --dry-run");
      Console.WriteLine("  publish --dry-run");
      Console.WriteLine("  help");
    }
  }
}
