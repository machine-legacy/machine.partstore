using System;

using DependencyStore.Domain.Core;
using DependencyStore.Domain.Core.Repositories;

namespace DependencyStore.Commands
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
      foreach (Repository repository in repositorySet.Repositories)
      {
        Console.WriteLine("Repository: {0}", repository.Name);
        foreach (ArchivedProject project in repository.Projects)
        {
          Console.WriteLine("{0,-30} {1} ({2} versions)", project.Name, project.LatestVersion.CreatedAt, project.Versions.Count);
        }
      }
      return CommandStatus.Success;
    }
  }
}