using System;
using System.Collections.Generic;

using DependencyStore.Domain.Configuration;

namespace DependencyStore.Domain.FileSystem.Repositories
{
  public interface IFileAndDirectoryRulesRepository
  {
    FileAndDirectoryRules FindDefault();
  }
}
