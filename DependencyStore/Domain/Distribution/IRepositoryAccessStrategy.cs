using System;

using DependencyStore.Domain.FileSystem;

namespace DependencyStore.Domain.Distribution
{
  public interface IRepositoryAccessStrategy
  {
    void CommitVersionToRepository(NewProjectVersion newProjectVersion);
    void CheckoutVersionFromRepository(ArchivedProjectVersion version, Purl directory);
    bool IsVersionPresentInRepository(ArchivedProjectVersion version);
  }
}