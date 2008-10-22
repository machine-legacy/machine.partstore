using System;
using System.Collections.Generic;

using Machine.Partstore.Application;
using Machine.Partstore.Commands;
using Machine.Partstore.Domain.Core;
using Machine.Partstore.Domain.FileSystem;
using Machine.Specifications;

using Rhino.Mocks;

namespace Machine.Partstore
{
  public class with_add_dependency_command : with_testing_repository_and_blank_project
  {
    protected static AddDependencyCommand command;

    Establish context = () =>
      command = container.Resolve.Object<AddDependencyCommand>();
  }

  [Subject("Adding dependencies")]
  public class when_adding_dependency_with_no_libraries_directory : with_add_dependency_command
  {
    static CommandStatus status;

    Because of = () =>
      status = command.Run();

    It should_fail = () =>
      status.ShouldEqual(CommandStatus.Failure);
  }

  [Subject("Adding dependencies")]
  public class when_adding_dependency_with_no_name_given : with_add_dependency_command
  {
    static CommandStatus status;

    Establish context = () =>
      project.AddLibraries();

    Because of = () =>
      status = command.Run();

    It should_fail = () =>
      status.ShouldEqual(CommandStatus.Failure);
  }

  [Subject("Adding dependencies")]
  public class when_adding_dependency_for_the_first_time : with_add_dependency_command
  {
    static CommandStatus status;

    Establish context = () =>
    {
      project.AddBuild();
      project.AddLibraries();
    };

    Because of = () =>
    {
      repository.AddVersion();
      command.ProjectName = "TestProject";
      status = command.Run();
    };

    It should_succeed = () =>
      status.ShouldEqual(CommandStatus.Success);
  }

  [Subject("Adding dependencies")]
  public class when_adding_dependency_that_is_missing_from_repository : with_configuration
  {
    static CurrentProject currentProject;
    static ProjectDependencies projectDependencies;
    static AddingDependencyResponse response;

    Establish context = () =>
    {
      RepositorySet repositorySet = New.RepositorySet().With(New.Repository("Test1"));
      currentProject = New.CurrentProject(New.ManifestStore(), repositorySet);
      
      services.CurrentProjectRepository.Stub(x => x.FindCurrentProject()).Return(currentProject);
      mocks.ReplayAll();

      projectDependencies = container.Resolve.Object<ProjectDependencies>();
    };

    Because of = () =>
      response = projectDependencies.AddDependency(String.Empty, "A");

    It should_not_match_project = () => 
      response.NoMatchingProject.ShouldBeTrue();

    It should_not_be_ambiguous = () => 
      response.AmbiguousProjectName.ShouldBeFalse();

    It should_not_save_project = () => 
      services.CurrentProjectRepository.AssertWasNotCalled(x => x.SaveCurrentProject(currentProject));

    It should_not_have_new_reference = () => 
      currentProject.References.ShouldBeEmpty();
  }

  [Subject("Adding dependencies")]
  public class when_adding_dependency : with_configuration
  {
    static CurrentProject currentProject;
    static ProjectDependencies projectDependencies;
    static AddingDependencyResponse response;

    Establish context = () =>
    {
      Purl repositoryPath = New.RandomPurl();
      RepositorySet repositorySet = New.RepositorySet().With(New.Repository("Test1").With(New.ArchivedProject("A").With(New.ArchivedProjectVersion(repositoryPath, "A", New.Version()))));
      currentProject = New.CurrentProject(New.ManifestStore(), repositorySet).WithLibrary(@"C:\Temp\Libraries");
      
      services.CurrentProjectRepository.Stub(x => x.FindCurrentProject()).Return(currentProject);
      mocks.ReplayAll();

      projectDependencies = container.Resolve.Object<ProjectDependencies>();
    };

    Because of = () =>
      response = projectDependencies.AddDependency(String.Empty, "A");

    It should_match_project = () => 
      response.NoMatchingProject.ShouldBeFalse();

    It should_not_be_ambiguous = () => 
      response.AmbiguousProjectName.ShouldBeFalse();

    It should_save_project = () => 
      services.CurrentProjectRepository.AssertWasCalled(x => x.SaveCurrentProject(currentProject));

    It should_have_new_reference = () => 
      currentProject.References.Count.ShouldEqual(1);
  }

  [Subject("Adding dependencies")]
  public class when_adding_dependency_that_is_in_multiple_repositories_and_no_repository_name_given : with_configuration
  {
    static CurrentProject currentProject;
    static ProjectDependencies projectDependencies;
    static AddingDependencyResponse response;

    Establish context = () =>
    {
      Purl repositoryPath = New.RandomPurl();
      Repository first = New.Repository("Test1").With(New.ArchivedProject("A").With(New.ArchivedProjectVersion(repositoryPath, "A", New.Version())));
      Repository second = New.Repository("Test2").With(New.ArchivedProject("A").With(New.ArchivedProjectVersion(repositoryPath, "A", New.Version())));
      RepositorySet repositorySet = New.RepositorySet().With(first).With(second);
      currentProject = New.CurrentProject(New.ManifestStore(), repositorySet).WithLibrary(@"C:\Temp\Libraries");
      
      services.CurrentProjectRepository.Stub(x => x.FindCurrentProject()).Return(currentProject);
      mocks.ReplayAll();

      projectDependencies = container.Resolve.Object<ProjectDependencies>();
    };

    Because of = () =>
      response = projectDependencies.AddDependency(String.Empty, "A");

    It should_be_ambiguous = () => 
      response.AmbiguousProjectName.ShouldBeTrue();

    It should_not_save_project = () => 
      services.CurrentProjectRepository.AssertWasNotCalled(x => x.SaveCurrentProject(currentProject));

    It should_not_have_new_reference = () => 
      currentProject.References.ShouldBeEmpty();
  }

  [Subject("Adding dependencies")]
  public class when_adding_dependency_that_is_in_multiple_repositories : with_configuration
  {
    static CurrentProject currentProject;
    static ProjectDependencies projectDependencies;
    static AddingDependencyResponse response;

    Establish context = () =>
    {
      Purl repositoryPath = New.RandomPurl();
      Repository first = New.Repository("Test1").With(New.ArchivedProject("A").With(New.ArchivedProjectVersion(repositoryPath, "A", New.Version())));
      Repository second = New.Repository("Test2").With(New.ArchivedProject("A").With(New.ArchivedProjectVersion(repositoryPath, "A", New.Version())));
      RepositorySet repositorySet = New.RepositorySet().With(first).With(second);
      currentProject = New.CurrentProject(New.ManifestStore(), repositorySet).WithLibrary(@"C:\Temp\Libraries");
      
      services.CurrentProjectRepository.Stub(x => x.FindCurrentProject()).Return(currentProject);
      mocks.ReplayAll();

      projectDependencies = container.Resolve.Object<ProjectDependencies>();
    };

    Because of = () =>
      response = projectDependencies.AddDependency("Test1", "A");

    It should_not_be_ambiguous = () => 
      response.AmbiguousProjectName.ShouldBeFalse();

    It should_save_project = () => 
      services.CurrentProjectRepository.AssertWasCalled(x => x.SaveCurrentProject(currentProject));

    It should_have_new_reference = () => 
      currentProject.References.Count.ShouldEqual(1);
  }

  [Subject("Adding dependencies")]
  public class when_adding_newer_dependency : with_configuration
  {
    static CurrentProject currentProject;
    static ProjectDependencies projectDependencies;
    static AddingDependencyResponse response;

    Establish context = () =>
    {
      VersionNumber older = New.Version();
      VersionNumber newer = New.Version();
      Purl repositoryPath = New.RandomPurl();
      RepositorySet repositorySet = New.RepositorySet().With(New.Repository("Test1").With(New.ArchivedProject("A").With(New.ArchivedProjectVersion(repositoryPath, "A", older), New.ArchivedProjectVersion(repositoryPath, "A", newer))));
      currentProject = New.CurrentProject(New.ManifestStore(New.Manifest("A", older)), repositorySet).WithLibrary(@"C:\Temp\Libraries");
      
      services.CurrentProjectRepository.Stub(x => x.FindCurrentProject()).Return(currentProject);
      mocks.ReplayAll();

      projectDependencies = container.Resolve.Object<ProjectDependencies>();
    };

    Because of = () =>
      response = projectDependencies.AddDependency(String.Empty, "A");

    It should_match_project = () => 
      response.NoMatchingProject.ShouldBeFalse();

    It should_not_be_ambiguous = () => 
      response.AmbiguousProjectName.ShouldBeFalse();

    It should_save_project = () => 
      services.CurrentProjectRepository.AssertWasCalled(x => x.SaveCurrentProject(currentProject));

    It should_have_same_number_of_references = () => 
      currentProject.References.Count.ShouldEqual(1);
  }
}
