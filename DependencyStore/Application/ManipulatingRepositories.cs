using System;
using System.Collections.Generic;

using DependencyStore.Domain.Core;
using DependencyStore.Domain.Core.Repositories;

namespace DependencyStore.Application
{
  public class ManipulatingRepositories : IManipulateRepositories
  {
    private readonly ICurrentProjectRepository _currentProjectRepository;
    private readonly IRepositoryRepository _repositoryRepository;
    private readonly IRepositorySetRepository _repositorySetRepository;

    public ManipulatingRepositories(ICurrentProjectRepository currentProjectRepository, IRepositoryRepository repositoryRepository, IRepositorySetRepository repositorySetRepository)
    {
      _currentProjectRepository = currentProjectRepository;
      _repositorySetRepository = repositorySetRepository;
      _repositoryRepository = repositoryRepository;
    }

    #region IManipulateRepositories Members
    public AddingVersionResponse AddNewVersion(string repositoryName, string tags)
    {
      CurrentProject project = _currentProjectRepository.FindCurrentProject();
      if (project.BuildDirectory.IsMissing)
      {
        return new AddingVersionResponse(true, false);
      }
      RepositorySet repositorySet = project.RepositorySet;
      Repository repository;
      if (repositorySet.HasMoreThanOneRepository)
      {
        if (String.IsNullOrEmpty(repositoryName))
        {
          return new AddingVersionResponse(false, true);
        }
        repository = repositorySet.FindRepositoryByName(repositoryName);
      }
      else
      {
        repository = repositorySet.DefaultRepository;
      }
      project.AddNewVersion(repository, new Tags(tags));
      _repositoryRepository.SaveRepository(repository);
      return new AddingVersionResponse(false, false);
    }
    
    public bool Refresh()
    {
      RepositorySet repositorySet = _repositorySetRepository.FindDefaultRepositorySet();
      repositorySet.Refresh();
      return true;
    }
    #endregion
  }
}
