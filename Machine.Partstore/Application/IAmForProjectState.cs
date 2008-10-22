using System;

namespace DependencyStore.Application
{
  public interface IAmForProjectState
  {
    CurrentProjectState GetCurrentProjectState();
    ConfigureResponse Configure(string defaultRepositoryName);
  }
}
