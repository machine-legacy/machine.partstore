using System;
using System.Collections.Generic;

using Machine.Container;

using DependencyStore.Commands;

namespace DependencyStore.CommandLine
{
  public class Program
  {
    public static void Main(string[] args)
    {
      using (MachineContainer container = new MachineContainer())
      {
        container.Initialize();
        container.PrepareForServices();
        ContainerRegistrationHelper helper = new ContainerRegistrationHelper(container);
        helper.AddServiceCollectionsFrom(typeof(DependencyStoreServices).Assembly);
        container.Start();
        IoC.Container = container;

        WriteVanityBanner();

        CommandLineParser parser = new CommandLineParser();
        parser.ParseCommandLine(args);
        string commandName = "help";
        if (parser.OrphanedArguments.Count > 0)
        {
          commandName = parser.OrphanedArguments[0].Value;
          parser.OrphanedArguments.RemoveAt(0);
        }

        CommandFactory commandFactory = new CommandFactory(container);
        commandFactory.AddCommand<ShowCommand>("show");
        commandFactory.AddCommand<UnpackageCommand>("unpackage");
        commandFactory.AddCommand<AddDependencyCommand>("add");
        commandFactory.AddCommand<PublishNewVersionCommand>("publish");
        commandFactory.AddCommand<PublishNewVersionCommand>("archive");
        commandFactory.AddCommand<SeachRepositoryCommand>("search");
        commandFactory.AddCommand<HelpCommand>("help");
        ICommand command = commandFactory.CreateCommand(commandName);
        
        CommandLineOptionBinder bind = new CommandLineOptionBinder(parser, command);
        bind.RequireFirst<AddDependencyCommand>(x => x.ProjectToAdd);
        
        command.Run();

        Console.WriteLine();
      }
    }

    private static void WriteVanityBanner()
    {
      Version version = typeof(Program).Assembly.GetName().Version;
      Console.WriteLine("DependencyStore {0} (C) Jacob Lewallen 2007,2008", version);
      Console.WriteLine();
    }
  }
}