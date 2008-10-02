using System;
using System.Collections.Generic;

using DependencyStore.Domain.Core;
using DependencyStore.Domain.Core.Repositories;

namespace DependencyStore.Commands
{
  public class AddDependencyCommand : Command
  {
    private readonly ICurrentProjectRepository _currentProjectRepository;
    private string _repositoryName;
    private string _projectName;

    public string RepositoryName
    {
      get { return _repositoryName; }
      set { _repositoryName = value; }
    }

    public string ProjectName
    {
      get { return _projectName; }
      set { _projectName = value; }
    }

    public AddDependencyCommand(ICurrentProjectRepository currentProjectRepository)
    {
      _currentProjectRepository = currentProjectRepository;
    }

    public override CommandStatus Run()
    {
      new ArchiveProgressDisplayer(false);
      CurrentProject project = _currentProjectRepository.FindCurrentProject();
      RepositorySet repositorySet = project.RepositorySet;
      List<ReferenceCandidate> candidates = FindReferenceCandidate(repositorySet);
      if (candidates.Count == 0)
      {
        Console.WriteLine("Project not found: {0}", this.ProjectName);
        return CommandStatus.Failure;
      }
      if (candidates.Count > 1)
      {
        Console.WriteLine("Too many projects found matching that criteria:");
        return CommandStatus.Failure;
      }
      ReferenceCandidate candidate = candidates[0];
      Console.WriteLine("Adding reference to {0} ({1})", candidate.ProjectName, candidate.VersionNumber.PrettyString);
      project.AddReference(repositorySet.FindArchivedProjectAndVersion(candidate));
      _currentProjectRepository.SaveCurrentProject(project);
      return CommandStatus.Success;
    }

    private List<ReferenceCandidate> FindReferenceCandidate(RepositorySet repositorySet)
    {
      List<ReferenceCandidate> found = new List<ReferenceCandidate>();
      ReferenceCandidate lookingFor = new ReferenceCandidate(this.ProjectName);
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