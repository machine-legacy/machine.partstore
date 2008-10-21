using System;

namespace DependencyStore.Application
{
  public interface IAmForProjectState
  {
    CurrentProjectState GetCurrentProjectState();
    bool Configure(string defaultRepositoryName);
  }
}
