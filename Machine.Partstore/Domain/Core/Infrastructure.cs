using System;
using System.Collections.Generic;

using Machine.Container;

using Machine.Partstore.Domain.Core.Repositories;

namespace Machine.Partstore.Domain.Core
{
  public class Infrastructure : Machine.Partstore.Domain.FileSystem.Infrastructure
  {
    public static IProjectManifestRepository ProjectManifestRepository
    {
      get { return IoC.Container.Resolve.Object<IProjectManifestRepository>(); }
    }

    public static IRepositoryRepository RepositoryRepository
    {
      get { return IoC.Container.Resolve.Object<IRepositoryRepository>(); }
    }
  }
}
