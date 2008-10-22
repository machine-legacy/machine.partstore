using System;

namespace Machine.Partstore.Application
{
  public interface IAmForProjectState
  {
    CurrentProjectState GetCurrentProjectState();
    ConfigureResponse Configure(string defaultRepositoryName);
  }
}
