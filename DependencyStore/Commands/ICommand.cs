using System;

namespace DependencyStore.Commands
{
  public interface ICommand
  {
    void Run();
  }
  public abstract class Command : ICommand
  {
    public abstract void Run();
  }
}