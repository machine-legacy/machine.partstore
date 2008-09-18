using System;
using System.Collections.Generic;

using Machine.Container;
using Machine.Core.Services;

using DependencyStore.Domain.Core.Repositories;

namespace DependencyStore.Domain.Core
{
  public class Infrastructure
  {
    public static IFileSystem FileSystem
    {
      get { return IoC.Container.Resolve.Object<IFileSystem>(); }
    }

    public static IFileSystemEntryRepository FileSystemEntryRepository
    {
      get { return IoC.Container.Resolve.Object<IFileSystemEntryRepository>(); }
    }
  }
}
