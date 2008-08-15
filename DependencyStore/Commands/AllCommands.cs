using System;
using System.Collections.Generic;

namespace DependencyStore.Commands
{
  public abstract class Command : ICommand
    {
      public abstract void Run();
    }
    public class ShowCommand : Command
    {
      public override void Run()
      {
      }
    }
    public class UnpackageCommand : Command
    {
      public override void Run()
      {
      }
    }
    public class AddDependencyCommand : Command
    {
      public override void Run()
      {
      }
    }
    public class AddNewVersionCommand : Command
    {
      public override void Run()
      {
      }
    }
    public class HelpCommand : Command
    {
      public override void Run()
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
