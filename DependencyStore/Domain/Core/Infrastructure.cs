using System;
using System.Collections.Generic;

using Machine.Container;
using Machine.Core.Services;

using DependencyStore.Domain.Repositories.Repositories;

namespace DependencyStore.Domain.Core
{
  public static class Infrastructure
  {
    public static IFileSystem FileSystem
    {
      get { return IoC.Container.Resolve.Object<IFileSystem>(); }
    }

    public static IProjectManifestRepository ProjectManifestRepository
    {
      get { return IoC.Container.Resolve.Object<IProjectManifestRepository>(); }
    }
  }
}
