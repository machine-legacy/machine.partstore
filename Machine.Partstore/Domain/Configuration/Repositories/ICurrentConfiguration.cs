using System;
using System.Collections.Generic;

namespace Machine.Partstore.Domain.Configuration.Repositories
{
  public interface ICurrentConfiguration
  {
    PartstoreConfiguration DefaultConfiguration
    {
      get;
    }
  }
}
