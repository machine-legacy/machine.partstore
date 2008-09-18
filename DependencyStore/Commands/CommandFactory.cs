using System;
using System.Collections.Generic;

using Machine.Container.Services;

namespace DependencyStore.Commands
{
  public class CommandFactory
  {
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
      foreach (RegisteredCommand registeredCommand in _commands)
      {
        if (registeredCommand.Name.Equals(name))
        {
          return (ICommand)_container.Resolve.Object(registeredCommand.Type);
        }
      }
      return _container.Resolve.Object<HelpCommand>();
    }
  }
}
