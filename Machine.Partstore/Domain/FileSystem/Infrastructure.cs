using System;
using System.Collections.Generic;

using Machine.Container;
using Machine.Core.Services;

using Machine.Partstore.Domain.FileSystem.Repositories;

namespace Machine.Partstore.Domain.FileSystem
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
