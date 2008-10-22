using System;
using System.Collections.Generic;

namespace DependencyStore.Application
{
  public interface IManipulateProjectDependencies
  {
    AddingDependencyResponse AddDependency(string fromRepositoryNamed, string projectName);
  }
}
