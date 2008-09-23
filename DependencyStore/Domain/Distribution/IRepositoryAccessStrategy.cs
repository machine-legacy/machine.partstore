using System;

using DependencyStore.Domain.Core;

namespace DependencyStore.Domain.Distribution
{
  public interface IRepositoryAccessStrategy
  {
    void CommitVersionToRepository(NewProjectVersion newProjectVersion);
    void CheckoutVersionFromRepository(ArchivedProjectVersion version, Purl directory);
    bool IsVersionPresentInRepository(ArchivedProjectVersion version);
  }
}