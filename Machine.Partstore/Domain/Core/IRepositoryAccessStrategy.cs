using System;

using Machine.Partstore.Domain.FileSystem;

namespace Machine.Partstore.Domain.Core
{
  public interface IRepositoryAccessStrategy
  {
    void CommitVersionToRepository(NewProjectVersion newProjectVersion);
    void CheckoutVersionFromRepository(ArchivedProjectVersion version, Purl directory);
    bool IsVersionPresentInRepository(ArchivedProjectVersion version);
  }
}