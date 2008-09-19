using System;
using System.Collections.Generic;
using DependencyStore.Domain.Configuration;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Layout;

using Machine.Container;
using Machine.Core.Utility;

using DependencyStore.Domain.Distribution;
using DependencyStore.Commands;

namespace DependencyStore.CommandLine
{
  public class Program
  {
    private static readonly ILog _log = LogManager.GetLogger(typeof(Program));

    public static void Main(string[] args)
    {
      ConfigureLog4net();
      WriteVanityBanner();

      using (MachineContainer container = new MachineContainer())
      {
        container.Initialize();
        container.PrepareForServices();
        ContainerRegistrationHelper helper = new ContainerRegistrationHelper(container);
        helper.AddServiceCollectionsFrom(typeof(DependencyStoreServices).Assembly);
        container.Start();
        IoC.Container = container;

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

        try
        {
          command.Run();
        }
        catch (ObjectNotFoundException objectNotFound)
        {
          Console.WriteLine("{0}", objectNotFound.Message);
          _log.Info(objectNotFound.Message);
        }
        Console.WriteLine();
      }
    }

    private static void WriteVanityBanner()
    {
      Version version = typeof(Program).Assembly.GetName().Version;
      string banner = String.Format("DependencyStore {0} (C) Jacob Lewallen 2008", version);
      Console.WriteLine(banner);
      Console.WriteLine();
      _log.Info(banner);
    }

    private static void ConfigureLog4net()
    {
      FileAppender appender = new FileAppender();
      appender.Layout = new PatternLayout("%d %-5p %c %m%n");
      
      appender.File = System.IO.Path.Combine(ConfigurationPaths.RootDataDirectory, "Ds.log");
      appender.AppendToFile = true;
      appender.ActivateOptions();
      BasicConfigurator.Configure(appender);
      LoggingHelper.Disable("Machine.Container");
    }
  }
}