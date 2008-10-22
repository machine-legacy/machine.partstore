using System;
using System.Collections.Generic;

namespace Machine.Partstore.Application
{
  public interface IManipulateProjectDependencies
  {
    AddingDependencyResponse AddDependency(string fromRepositoryNamed, string projectName);
  }
}
