using System;

using Machine.Partstore.Domain.Core;
using Machine.Partstore.Domain.Core.Repositories;

namespace Machine.Partstore.Commands
{
  public class SeachRepositoryCommand : Command
  {
    private readonly IRepositorySetRepository _repositorySetRepository;

    public SeachRepositoryCommand(IRepositorySetRepository repositorySetRepository)
    {
      _repositorySetRepository = repositorySetRepository;
    }

    public override CommandStatus Run()
    {
      RepositorySet repositorySet = _repositorySetRepository.FindDefaultRepositorySet();
      foreach (ReferenceCandidate candidate in repositorySet.FindAllReferenceCandidates())
      {
        Console.WriteLine("{0,-10} {1,-30} {2,-20} {3,-25}", candidate.RepositoryName, candidate.ProjectName, candidate.PrettyAge, candidate.VersionNumber.TimeStamp.ToLocalTime());
      }
      return CommandStatus.Success;
    }
  }
}