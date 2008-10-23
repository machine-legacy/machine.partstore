using System;

using Machine.Partstore.Domain.FileSystem;

namespace Machine.Partstore.Domain.Core
{
  public interface IRepositoryAccessStrategy
  {
    void CommitVersionToRepository(Repository repository, NewProjectVersion newProjectVersion);
    void CheckoutVersionFromRepository(Repository repository, ArchivedProjectVersion version, Purl directory);
    bool IsVersionPresentInRepository(Repository repository, ArchivedProjectVersion version);
  }
}