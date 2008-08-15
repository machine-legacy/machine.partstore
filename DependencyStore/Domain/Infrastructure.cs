using System;
using System.Collections.Generic;

using DependencyStore.Services.DataAccess;

using Machine.Container;
using Machine.Core.Services;

namespace DependencyStore.Domain
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
