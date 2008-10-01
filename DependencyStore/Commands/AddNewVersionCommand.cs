using System;
using System.Collections.Generic;

using DependencyStore.Domain.Core;
using DependencyStore.Domain.Core.Repositories;

namespace DependencyStore.Commands
{
  public class AddNewVersionCommand : Command
  {
    private readonly ICurrentProjectRepository _currentProjectRepository;
    private readonly IRepositorySetRepository _repositorySetRepository;
    private readonly IRepositoryRepository _repositoryRepository;
    private string _repositoryName;
    private string _tags;

    public string RepositoryName
    {
      get { return _repositoryName; }
      set { _repositoryName = value; }
    }

    public string Tags
    {
      get { return _tags; }
      set { _tags = value; }
    }

    public AddNewVersionCommand(ICurrentProjectRepository currentProjectRepository, IRepositorySetRepository repositorySetRepository, IRepositoryRepository repositoryRepository)
    {
      _currentProjectRepository = currentProjectRepository;
      _repositoryRepository = repositoryRepository;
      _repositorySetRepository = repositorySetRepository;
    }

    public override CommandStatus Run()
    {
      new ArchiveProgressDisplayer(true);
      CurrentProject project = _currentProjectRepository.FindCurrentProject();
      if (!project.HasBuildDirectory)
      {
        Console.WriteLine("Current project has no Build directory configured.");
        return CommandStatus.Failure;
      }
      RepositorySet repositorySet = _repositorySetRepository.FindDefaultRepositorySet();
      Repository repository = repositorySet.DefaultRepository;
      project.AddNewVersion(repository, new Tags(_tags));
      _repositoryRepository.SaveRepository(repository);
      return CommandStatus.Success;
    }
  }
}