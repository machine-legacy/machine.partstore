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

    public override void Run()
    {
      Repository repository = _repositoryRepository.FindDefaultRepository();
      foreach (ArchivedProject project in repository.Projects)
      {
        Console.WriteLine("{0,-30} {1}", project.Name, project.LatestVersion.CreatedAt);
      }
    }
  }
}