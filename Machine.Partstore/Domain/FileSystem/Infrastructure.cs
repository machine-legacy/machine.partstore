using System;
using System.Collections.Generic;

using Machine.Container;
using Machine.Core.Services;

using DependencyStore.Domain.FileSystem.Repositories;

namespace DependencyStore.Domain.FileSystem
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
