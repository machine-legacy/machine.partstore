using System;
using System.Collections.Generic;

using DependencyStore.Domain.Core;
using DependencyStore.Domain.Core.Repositories;

namespace DependencyStore.Application
{
  public class ProjectDependencies : IManipulateProjectDependencies
  {
    private readonly ICurrentProjectRepository _currentProjectRepository;

    public ProjectDependencies(ICurrentProjectRepository currentProjectRepository)
    {
      _currentProjectRepository = currentProjectRepository;
    }

    #region IManipulateProjectDependencies Members
    public AddingDependencyResponse AddDependency(string fromRepositoryNamed, string projectName)
    {
      CurrentProject project = _currentProjectRepository.FindCurrentProject();
      RepositorySet repositorySet = project.RepositorySet;
      List<ReferenceCandidate> candidates = FindReferenceCandidate(repositorySet, projectName);
      if (candidates.Count == 0)
      {
        return new AddingDependencyResponse(true, false);
      }
      if (candidates.Count > 1)
      {
        return new AddingDependencyResponse(false, true);
      }
      ReferenceCandidate candidate = candidates[0];
      Console.WriteLine("Adding reference to {0} ({1})", candidate.ProjectName, candidate.VersionNumber.PrettyString);
      project.AddReference(repositorySet.FindArchivedProjectAndVersion(candidate));
      _currentProjectRepository.SaveCurrentProject(project);
      return new AddingDependencyResponse(false, false);
    }
    #endregion

    private static List<ReferenceCandidate> FindReferenceCandidate(RepositorySet repositorySet, string projectName)
    {
      List<ReferenceCandidate> found = new List<ReferenceCandidate>();
      ReferenceCandidate lookingFor = new ReferenceCandidate(projectName);
      foreach (ReferenceCandidate candidate in repositorySet.FindAllReferenceCandidates())
      {
        if (candidate.Equals(lookingFor))
        {
          found.Add(candidate);
        }
      }
      return found;
    }
  }
}
