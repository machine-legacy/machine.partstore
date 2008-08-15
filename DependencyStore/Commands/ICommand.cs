using DependencyStore.Domain.Configuration;

namespace DependencyStore.Commands
{
  public interface ICommand
  {
    void Run(DependencyStoreConfiguration configuration);
  }
}