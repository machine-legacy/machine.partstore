using System;
using System.Collections.Generic;

using Machine.Partstore.Domain.Configuration;

namespace Machine.Partstore.Domain.FileSystem.Repositories
{
  public interface IFileAndDirectoryRulesRepository
  {
    FileAndDirectoryRules FindDefault();
  }
}
