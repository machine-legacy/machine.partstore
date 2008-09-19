using System;

using DependencyStore.Domain.Distribution;
using DependencyStore.Domain.Distribution.Repositories;

namespace DependencyStore.Commands
{
  public class SeachRepositoryCommand : Command
  {
    private readonly IRepositoryRepository _repositoryRepository;

    public SeachRepositoryCommand(IRepositoryRepository repositoryRepository)
    {
      _repositoryRepository = repositoryRepository;
    }

    public override CommandStatus Run()
    {
      Repository repository = _repositoryRepository.FindDefaultRepository();
      foreach (ArchivedProject project in repository.Projects)
      {
        Console.WriteLine("{0,-30} {1} ({2} versions)", project.Name, project.LatestVersion.CreatedAt, project.Versions.Count);
      }
      return CommandStatus.Success;
    }
  }
}