using System;
using System.Collections.Generic;

namespace Machine.Partstore.Application
{
  public interface IManipulateRepositorySets
  {
    AddingVersionResponse AddNewVersion(string repositoryName, string tags);
    bool Refresh();
  }
}
