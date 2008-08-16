using System;
using System.Collections.Generic;

using DependencyStore.Domain.Configuration;

namespace DependencyStore.Services.DataAccess
{
  public interface IFileAndDirectoryRulesRepository
  {
    FileAndDirectoryRules FindDefault();
  }
}
