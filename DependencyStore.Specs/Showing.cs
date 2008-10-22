using System;
using System.Collections.Generic;
using System.Linq;

using DependencyStore.Application;
using DependencyStore.Commands;
using DependencyStore.Domain.Core;
using DependencyStore.Domain.FileSystem;
using Machine.Specifications;
using Rhino.Mocks;

namespace DependencyStore
{
  public class with_show_command : with_testing_repository_and_blank_project
  {
    protected static ShowCommand command;

    Establish context = () =>
      command = container.Resolve.Object<ShowCommand>();
  }

  [Subject("Showing")]
  public class when_showing_a_blank_directory : with_testing_repository_and_blank_directory
  {
    static CommandStatus status;
    static ShowCommand command;

    Establish context = () =>
      command = container.Resolve.Object<ShowCommand>();

    Because of = () =>
      status = command.Run();

    It should_fail = () =>
      status.ShouldEqual(CommandStatus.Failure);
  }

  [Subject("Showing")]
  public class when_showing_blank_project : with_show_command
  {
    static CommandStatus status;

    Because of = () =>
      status = command.Run();

    It should_succeed = () =>
      status.ShouldEqual(CommandStatus.Success);
  }

  [Subject("Current project state")]
  public class with_no_configuration : with_configuration
  {
    static ProjectState projectState;
    static CurrentProjectState state;

    Establish context = () =>
    {
      SetupResult.For(services.ConfigurationRepository.FindProjectConfiguration()).Return(null);
      mocks.ReplayAll();

      projectState = container.Resolve.Object<ProjectState>();
    };

    Because of = () =>
      state = projectState.GetCurrentProjectState();

    It should_have_missing_configuration = () => 
      state.MissingConfiguration.ShouldBeTrue();

    It should_have_no_references = () => 
      state.References.ShouldBeEmpty();
  }

  [Subject("Current project state")]
  public class with_no_dependencies : with_configuration
  {
    static CurrentProject currentProject;
    static ProjectState projectState;
    static CurrentProjectState state;

    Establish context = () =>
    {
      currentProject = New.CurrentProject();
      SetupResult.For(services.ConfigurationRepository.FindProjectConfiguration()).Return(configuration);
      SetupResult.For(services.CurrentProjectRepository.FindCurrentProject()).Return(currentProject);
      mocks.ReplayAll();

      projectState = container.Resolve.Object<ProjectState>();
    };

    Because of = () =>
      state = projectState.GetCurrentProjectState();

    It should_have_configuration = () => 
      state.MissingConfiguration.ShouldBeFalse();

    It should_have_no_references = () => 
      state.References.ShouldBeEmpty();

    It should_have_project_name = () =>
      state.ProjectName.ShouldEqual("TestProject");
  }

  [Subject("Current project state")]
  public class with_dependencies_missing_from_repository : with_configuration
  {
    static CurrentProject currentProject;
    static RepositorySet repositorySet;
    static ProjectState projectState;
    static CurrentProjectState state;

    Establish context = () =>
    {
      repositorySet = New.RepositorySet();
      currentProject = New.CurrentProject(New.ManifestStore(New.Manifest("A")), repositorySet);
      SetupResult.For(services.ConfigurationRepository.FindProjectConfiguration()).Return(configuration);
      SetupResult.For(services.CurrentProjectRepository.FindCurrentProject()).Return(currentProject);
      mocks.ReplayAll();

      projectState = container.Resolve.Object<ProjectState>();
    };

    Because of = () =>
      state = projectState.GetCurrentProjectState();

    It should_have_configuration = () => 
      state.MissingConfiguration.ShouldBeFalse();

    It should_have_references = () => 
      state.References.Count.ShouldEqual(1);

    It should_have_reference_to_a = () =>
      state.References.First().DependencyName.ShouldEqual("A");

    It should_have_reference_to_a_that_is_unhealthy = () =>
      state.References.First().IsHealthy.ShouldBeFalse();

    It should_have_reference_to_a_that_is_missing_project = () =>
      state.References.First().IsProjectMissing.ShouldBeTrue();

    It should_have_reference_to_a_that_is_missing_version = () =>
      state.References.First().IsReferencedVersionMissing.ShouldBeTrue();

    It should_have_reference_to_a_with_no_versions_installed = () =>
      state.References.First().IsAnyVersionInstalled.ShouldBeFalse();

    It should_have_project_name = () =>
      state.ProjectName.ShouldEqual("TestProject");
  }

  [Subject("Current project state")]
  public class with_dependencies_missing_versions_in_repository : with_configuration
  {
    static CurrentProject currentProject;
    static RepositorySet repositorySet;
    static ProjectState projectState;
    static CurrentProjectState state;

    Establish context = () =>
    {
      repositorySet = New.RepositorySet().With(New.Repository().With(New.ArchivedProject("A")));
      currentProject = New.CurrentProject(New.ManifestStore(New.Manifest("A")), repositorySet);
      SetupResult.For(services.ConfigurationRepository.FindProjectConfiguration()).Return(configuration);
      SetupResult.For(services.CurrentProjectRepository.FindCurrentProject()).Return(currentProject);
      mocks.ReplayAll();

      projectState = container.Resolve.Object<ProjectState>();
    };

    Because of = () =>
      state = projectState.GetCurrentProjectState();

    It should_have_configuration = () => 
      state.MissingConfiguration.ShouldBeFalse();

    It should_have_references = () => 
      state.References.Count.ShouldEqual(1);

    It should_have_reference_to_a = () =>
      state.References.First().DependencyName.ShouldEqual("A");

    It should_have_reference_to_a_that_is_unhealthy = () =>
      state.References.First().IsHealthy.ShouldBeFalse();

    It should_have_reference_to_a_that_is_missing_project = () =>
      state.References.First().IsProjectMissing.ShouldBeFalse();

    It should_have_reference_to_a_that_is_missing_version = () =>
      state.References.First().IsReferencedVersionMissing.ShouldBeTrue();

    It should_have_reference_to_a_with_no_versions_installed = () =>
      state.References.First().IsAnyVersionInstalled.ShouldBeFalse();

    It should_have_project_name = () =>
      state.ProjectName.ShouldEqual("TestProject");
  }

  [Subject("Current project state")]
  public class with_dependencies_that_are_in_repository : with_configuration
  {
    static CurrentProject currentProject;
    static RepositorySet repositorySet;
    static ProjectState projectState;
    static CurrentProjectState state;

    Establish context = () =>
    {
      ProjectManifestStore installed = New.ManifestStore();
      VersionNumber version = New.Version();
      Purl repositoryPath = New.RandomPurl();
      repositorySet = New.RepositorySet().With(New.Repository().With(New.ArchivedProject("A").With(New.ArchivedProjectVersion(repositoryPath, "A", version))));
      currentProject = New.CurrentProject(New.ManifestStore(New.Manifest("A", version)), repositorySet).WithLibrary(@"C:\Temp\Libraries");
      
      SetupResult.For(services.ConfigurationRepository.FindProjectConfiguration()).Return(configuration);
      SetupResult.For(services.CurrentProjectRepository.FindCurrentProject()).Return(currentProject);
      SetupResult.For(services.ProjectManifestRepository.FindProjectManifestStore(Purl.For(@"C:\Temp\Libraries"))).Return(installed);
      SetupResult.For(services.ProjectManifestRepository.FindProjectManifestStore(Purl.For(@"C:\Temp\Libraries\A"))).Return(New.ManifestStore());
      mocks.ReplayAll();

      projectState = container.Resolve.Object<ProjectState>();
    };

    Because of = () =>
      state = projectState.GetCurrentProjectState();

    It should_have_configuration = () => 
      state.MissingConfiguration.ShouldBeFalse();

    It should_have_references = () => 
      state.References.Count.ShouldEqual(1);

    It should_have_reference_to_dependency = () =>
      state.References.First().DependencyName.ShouldEqual("A");

    It should_have_reference_that_is_healthy = () =>
      state.References.First().IsHealthy.ShouldBeTrue();

    It should_have_reference_that_is_up_to_date = () =>
      state.References.First().IsToLatestVersion.ShouldBeTrue();

    It should_have_reference_with_no_versions_installed = () =>
      state.References.First().IsAnyVersionInstalled.ShouldBeFalse();

    It should_have_project_name = () =>
      state.ProjectName.ShouldEqual("TestProject");
  }
}
