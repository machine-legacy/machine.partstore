using System;
using System.Collections.Generic;

namespace DependencyStore.Application
{
  public interface IManipulateRepositories
  {
    AddingVersionResponse AddNewVersion(string repositoryName, string tags);
    bool Refresh();
  }
}
