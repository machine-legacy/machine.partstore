using System;

namespace Machine.Partstore.Commands
{
  public class HelpCommand : Command
  {
    public override CommandStatus Run()
    {
      Console.WriteLine("{0} <command> [options]", "Parts.exe");
      Console.WriteLine("Commands:");
      Console.WriteLine("  show");
      Console.WriteLine("  unpackage --dry-run");
      Console.WriteLine("  add --dry-run");
      Console.WriteLine("  update --all --dry-run");
      Console.WriteLine("  publish --dry-run");
      Console.WriteLine("  search");
      Console.WriteLine("  config");
      Console.WriteLine("  help");
      return CommandStatus.Success;
    }
  }
}