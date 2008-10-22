using System;

namespace Machine.Partstore.Commands
{
  public enum CommandStatus
  {
    Success,
    Failure
  }
  public interface ICommand
  {
    CommandStatus Run();
  }
  public abstract class Command : ICommand
  {
    public abstract CommandStatus Run();
  }
}