using System;
using System.Collections.Generic;

using Machine.Container;

using DependencyStore.Domain.Repositories.Repositories;

namespace DependencyStore.Domain.Repositories
{
  public class Infrastructure : DependencyStore.Domain.Core.Infrastructure
  {
    public static IProjectManifestRepository ProjectManifestRepository
    {
      get { return IoC.Container.Resolve.Object<IProjectManifestRepository>(); }
    }

    public static IProjectReferenceRepository ProjectReferenceRepository
    {
      get { return IoC.Container.Resolve.Object<IProjectReferenceRepository>(); }
    }
  }
}
