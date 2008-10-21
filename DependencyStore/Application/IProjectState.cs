using System;

namespace DependencyStore.Application
{
  public interface IProjectState
  {
    CurrentProjectState GetCurrentProjectState();
    bool Configure(string defaultRepositoryName);
  }
}
