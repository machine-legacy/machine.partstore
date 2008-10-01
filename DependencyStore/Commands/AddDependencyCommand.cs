using System;
using System.Collections.Generic;
using DependencyStore.Domain.Core;
using DependencyStore.Domain.Core.Repositories;

namespace DependencyStore.Commands
{
  public class AddDependencyCommand : Command
  {
    private readonly ICurrentProjectRepository _currentProjectRepository;
    private readonly IRepositorySetRepository _repositorySetRepository;
    private string _projectToAdd;

    public string ProjectToAdd
    {
      get { return _projectToAdd; }
      set { _projectToAdd = value; }
    }

    public AddDependencyCommand(ICurrentProjectRepository currentProjectRepository, IRepositorySetRepository repositorySetRepository)
    {
      _repositorySetRepository = repositorySetRepository;
      _currentProjectRepository = currentProjectRepository;
    }

    public override CommandStatus Run()
    {
      new ArchiveProgressDisplayer(false);
      RepositorySet repositorySet = _repositorySetRepository.FindDefaultRepositorySet();
      List<ReferenceCandidate> candidates = FindReferenceCandidate(repositorySet);
      if (candidates.Count == 0)
      {
        Console.WriteLine("Project not found: {0}", this.ProjectToAdd);
        return CommandStatus.Failure;
      }
      if (candidates.Count > 1)
      {
        Console.WriteLine("Too many projects found matching that criteria:");
        return CommandStatus.Failure;
      }
      ReferenceCandidate candidate = candidates[0];
      Console.WriteLine("Adding reference to {0} ({1})", candidate.ProjectName, candidate.VersionNumber.PrettyString);
      CurrentProject project = _currentProjectRepository.FindCurrentProject();
      project.AddReference(repositorySet, candidate);
      _currentProjectRepository.SaveCurrentProject(project, repositorySet);
      return CommandStatus.Success;
    }

    private List<ReferenceCandidate> FindReferenceCandidate(RepositorySet repositorySet)
    {
      List<ReferenceCandidate> found = new List<ReferenceCandidate>();
      ReferenceCandidate lookingFor = new ReferenceCandidate(this.ProjectToAdd);
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