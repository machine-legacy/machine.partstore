using System;
using System.Collections.Generic;
using Machine.Core.Services;

namespace Machine.Partstore.Domain.Core.Repositories.Impl
{
  public class HooksRepository : IHooksRepository
  {
    private readonly IFileSystem _fileSystem;

    public HooksRepository(IFileSystem fileSystem)
    {
      _fileSystem = fileSystem;
    }

    #region IHooksRepository Members
    public Hooks CreateHooks(Repository repository)
    {
      return new Hooks(_fileSystem, repository.RootPath.Join("Hooks"));
    }
    #endregion
  }
}
