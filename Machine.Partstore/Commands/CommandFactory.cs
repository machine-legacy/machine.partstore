using System;
using System.Collections.Generic;

using Machine.Container.Services;

namespace DependencyStore.Commands
{
  public class CommandFactory
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(CommandFactory));
    private readonly IMachineContainer _container;
    private readonly List<RegisteredCommand> _commands = new List<RegisteredCommand>();

    private class RegisteredCommand
    {
      public string Name;
      public Type Type;
      public RegisteredCommand(string name, Type type)
      {
        this.Name = name;
        this.Type = type;
      }
    }

    public CommandFactory(IMachineContainer container)
    {
      _container = container;
    }

    public void AddCommand<T>(string name) where T : ICommand
    {
      _commands.Add(new RegisteredCommand(name, typeof (T)));
    }

    public ICommand CreateCommand(string name)
    {
      Type commandType = typeof(HelpCommand);
      foreach (RegisteredCommand registeredCommand in _commands)
      {
        if (registeredCommand.Name.Equals(name))
        {
          commandType = registeredCommand.Type;
          break;
        }
      }
      ICommand command = (ICommand)_container.Resolve.Object(commandType);
      _log.Info("Using: " + command);
      return command;
    }
  }
}
