using System;
using System.Collections.Generic;

namespace Machine.Partstore.Domain.Core.Repositories
{
  public interface IHooksRepository
  {
    Hooks CreateHooks(Repository repository);
  }
}
