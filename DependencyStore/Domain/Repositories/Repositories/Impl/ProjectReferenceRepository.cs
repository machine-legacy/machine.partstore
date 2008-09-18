﻿using System;
using System.Collections.Generic;

using DependencyStore.Domain.Core;
using DependencyStore.Domain.Core.Repositories;

namespace DependencyStore.Domain.Repositories.Repositories.Impl
{
  public class ProjectReferenceRepository : IProjectReferenceRepository
  {
    private readonly IRepositoryRepository _repositoryRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IProjectManifestRepository _projectManifestRepository;

    public ProjectReferenceRepository(IRepositoryRepository repositoryRepository, IProjectRepository projectRepository, IProjectManifestRepository projectManifestRepository)
    {
      _repositoryRepository = repositoryRepository;
      _projectRepository = projectRepository;
      _projectManifestRepository = projectManifestRepository;
    }

    public IList<ProjectReference> FindAllProjectReferences()
    {
      Repository repository = _repositoryRepository.FindDefaultRepository();
      List<ProjectReference> references = new List<ProjectReference>();
      foreach (Project project in _projectRepository.FindAllProjects())
      {
        foreach (ProjectManifest manifest in _projectManifestRepository.FindProjectManifests(project))
        {
          ArchivedProject archivedProject = repository.FindProject(manifest);
          if (archivedProject == null)
          {
            throw new InvalidOperationException("Missing project: " + manifest);
          }
          ArchivedProjectVersion version = archivedProject.LatestVersion;
          if (version == null)
          {
            throw new InvalidOperationException("Missing version: " + manifest);
          }
          references.Add(new ProjectReference(archivedProject, project, version, manifest));
        }
      }
      return references;
    }
  }
}