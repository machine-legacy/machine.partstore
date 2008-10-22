using System;
using System.Collections.Generic;
using System.Linq;

using Machine.Partstore.Application;
using Machine.Partstore.Commands;
using Machine.Partstore.Domain.Core;
using Machine.Partstore.Domain.FileSystem;
using Machine.Specifications;
using Rhino.Mocks;

namespace Machine.Partstore
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
      services.ConfigurationRepository.Stub(x => x.FindProjectConfiguration()).Return(null);
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
    static ProjectState projectState;
    static CurrentProjectState state;

    Establish context = () =>
    {
      CurrentProject currentProject = New.CurrentProject();
      services.ConfigurationRepository.Stub(x => x.FindProjectConfiguration()).Return(configuration);
      services.CurrentProjectRepository.Stub(x => x.FindCurrentProject()).Return(currentProject);
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
    static ProjectState projectState;
    static CurrentProjectState state;

    Establish context = () =>
    {
      RepositorySet repositorySet = New.RepositorySet();
      CurrentProject currentProject = New.CurrentProject(New.ManifestStore(New.Manifest("A")), repositorySet);
      services.ConfigurationRepository.Stub(x => x.FindProjectConfiguration()).Return(configuration);
      services.CurrentProjectRepository.Stub(x => x.FindCurrentProject()).Return(currentProject);
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
    static ProjectState projectState;
    static CurrentProjectState state;

    Establish context = () =>
    {
      RepositorySet repositorySet = New.RepositorySet().With(New.Repository().With(New.ArchivedProject("A")));
      CurrentProject currentProject = New.CurrentProject(New.ManifestStore(New.Manifest("A")), repositorySet);
      services.ConfigurationRepository.Stub(x => x.FindProjectConfiguration()).Return(configuration);
      services.CurrentProjectRepository.Stub(x => x.FindCurrentProject()).Return(currentProject);
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
    static ProjectState projectState;
    static CurrentProjectState state;

    Establish context = () =>
    {
      VersionNumber version = New.Version();
      Purl repositoryPath = New.RandomPurl();
      ProjectManifestStore required = New.ManifestStore(New.Manifest("A", version));
      RepositorySet repositorySet = New.RepositorySet().With(New.Repository().With(New.ArchivedProject("A").With(New.ArchivedProjectVersion(repositoryPath, "A", version))));
      CurrentProject currentProject = New.CurrentProject(required, repositorySet).WithLibrary(@"C:\Temp\Libraries");
      
      services.ConfigurationRepository.Stub(x => x.FindProjectConfiguration()).Return(configuration);
      services.CurrentProjectRepository.Stub(x => x.FindCurrentProject()).Return(currentProject);
      services.ProjectManifestRepository.Stub(x => x.FindProjectManifestStore(Purl.For(@"C:\Temp\Libraries"))).Return(required);
      services.ProjectManifestRepository.Stub(x => x.FindProjectManifestStore(Purl.For(@"C:\Temp\Libraries\A"))).Return(New.ManifestStore());
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

  [Subject("Current project state")]
  public class with_dependencies_that_are_in_repository_and_installed : with_configuration
  {
    static ProjectState projectState;
    static CurrentProjectState state;

    Establish context = () =>
    {
      VersionNumber version = New.Version();
      Purl repositoryPath = New.RandomPurl();
      ProjectManifestStore required = New.ManifestStore(New.Manifest("A", version));
      RepositorySet repositorySet = New.RepositorySet().With(New.Repository().With(New.ArchivedProject("A").With(New.ArchivedProjectVersion(repositoryPath, "A", version))));
      CurrentProject currentProject = New.CurrentProject(required, repositorySet).WithLibrary(@"C:\Temp\Libraries");
      
      services.ConfigurationRepository.Stub(x => x.FindProjectConfiguration()).Return(configuration);
      services.CurrentProjectRepository.Stub(x => x.FindCurrentProject()).Return(currentProject);
      services.ProjectManifestRepository.Stub(x => x.FindProjectManifestStore(Purl.For(@"C:\Temp\Libraries"))).Return(required);
      services.ProjectManifestRepository.Stub(x => x.FindProjectManifestStore(Purl.For(@"C:\Temp\Libraries\A"))).Return(required);
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
      state.References.First().IsAnyVersionInstalled.ShouldBeTrue();

    It should_have_reference_with_desired_version_installed = () =>
      state.References.First().IsReferencedVersionInstalled.ShouldBeTrue();

    It should_have_project_name = () =>
      state.ProjectName.ShouldEqual("TestProject");
  }

  [Subject("Current project state")]
  public class with_dependencies_that_are_in_repository_and_older_version_is_installed : with_configuration
  {
    static ProjectState projectState;
    static CurrentProjectState state;

    Establish context = () =>
    {
      VersionNumber older = New.Version();
      VersionNumber newer = New.Version();
      Purl repositoryPath = New.RandomPurl();
      ProjectManifestStore required = New.ManifestStore(New.Manifest("A", newer));
      ProjectManifestStore installed = New.ManifestStore(New.Manifest("A", older));
      RepositorySet repositorySet = New.RepositorySet().With(New.Repository().With(New.ArchivedProject("A").With(New.ArchivedProjectVersion(repositoryPath, "A", older), New.ArchivedProjectVersion(repositoryPath, "A", newer))));
      CurrentProject currentProject = New.CurrentProject(required, repositorySet).WithLibrary(@"C:\Temp\Libraries");
      
      services.ConfigurationRepository.Stub(x => x.FindProjectConfiguration()).Return(configuration);
      services.CurrentProjectRepository.Stub(x => x.FindCurrentProject()).Return(currentProject);
      services.ProjectManifestRepository.Stub(x => x.FindProjectManifestStore(Purl.For(@"C:\Temp\Libraries"))).Return(required);
      services.ProjectManifestRepository.Stub(x => x.FindProjectManifestStore(Purl.For(@"C:\Temp\Libraries\A"))).Return(installed);
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

    It should_have_reference_that_is_to_older_version = () =>
      state.References.First().IsToLatestVersion.ShouldBeTrue();

    It should_have_reference_with_is_up_to_date_because_newer_is_required_older_is_installed = () =>
      state.References.First().IsOutdated.ShouldBeFalse();

    It should_have_reference_with_older_version_installed = () =>
      state.References.First().IsOlderVersionInstalled.ShouldBeTrue();

    It should_have_reference_with_any_version_installed = () =>
      state.References.First().IsAnyVersionInstalled.ShouldBeTrue();

    It should_have_project_name = () =>
      state.ProjectName.ShouldEqual("TestProject");
  }

  [Subject("Current project state")]
  public class with_dependencies_that_are_in_repository_and_older_missing_version_installed : with_configuration
  {
    static ProjectState projectState;
    static CurrentProjectState state;

    Establish context = () =>
    {
      VersionNumber older = New.Version();
      VersionNumber newer = New.Version();
      Purl repositoryPath = New.RandomPurl();
      ProjectManifestStore required = New.ManifestStore(New.Manifest("A", newer));
      ProjectManifestStore installed = New.ManifestStore(New.Manifest("A", older));
      RepositorySet repositorySet = New.RepositorySet().With(New.Repository().With(New.ArchivedProject("A").With(New.ArchivedProjectVersion(repositoryPath, "A", older), New.ArchivedProjectVersion(repositoryPath, "A", newer))));
      CurrentProject currentProject = New.CurrentProject(required, repositorySet).WithLibrary(@"C:\Temp\Libraries");
      
      services.ConfigurationRepository.Stub(x => x.FindProjectConfiguration()).Return(configuration);
      services.CurrentProjectRepository.Stub(x => x.FindCurrentProject()).Return(currentProject);
      services.ProjectManifestRepository.Stub(x => x.FindProjectManifestStore(Purl.For(@"C:\Temp\Libraries"))).Return(required);
      services.ProjectManifestRepository.Stub(x => x.FindProjectManifestStore(Purl.For(@"C:\Temp\Libraries\A"))).Return(installed);
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

    It should_have_reference_that_is_to_older_version = () =>
      state.References.First().IsToLatestVersion.ShouldBeTrue();

    It should_have_reference_with_older_version_installed = () =>
      state.References.First().IsOlderVersionInstalled.ShouldBeTrue();

    It should_have_reference_with_any_version_installed = () =>
      state.References.First().IsAnyVersionInstalled.ShouldBeTrue();

    It should_have_project_name = () =>
      state.ProjectName.ShouldEqual("TestProject");
  }

  [Subject("Current project state")]
  public class with_dependencies_that_is_old : with_configuration
  {
    static ProjectState projectState;
    static CurrentProjectState state;

    Establish context = () =>
    {
      VersionNumber older = New.Version();
      VersionNumber newer = New.Version();
      Purl repositoryPath = New.RandomPurl();
      ProjectManifestStore required = New.ManifestStore(New.Manifest("A", older));
      ProjectManifestStore installed = New.ManifestStore(New.Manifest("A", older));
      RepositorySet repositorySet = New.RepositorySet().With(New.Repository().With(New.ArchivedProject("A").With(New.ArchivedProjectVersion(repositoryPath, "A", older), New.ArchivedProjectVersion(repositoryPath, "A", newer))));
      CurrentProject currentProject = New.CurrentProject(required, repositorySet).WithLibrary(@"C:\Temp\Libraries");
      
      services.ConfigurationRepository.Stub(x => x.FindProjectConfiguration()).Return(configuration);
      services.CurrentProjectRepository.Stub(x => x.FindCurrentProject()).Return(currentProject);
      services.ProjectManifestRepository.Stub(x => x.FindProjectManifestStore(Purl.For(@"C:\Temp\Libraries"))).Return(required);
      services.ProjectManifestRepository.Stub(x => x.FindProjectManifestStore(Purl.For(@"C:\Temp\Libraries\A"))).Return(installed);
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

    It should_have_reference_that_is_to_older_version = () =>
      state.References.First().IsToLatestVersion.ShouldBeFalse();

    It should_have_reference_with_is_outdated = () =>
      state.References.First().IsOutdated.ShouldBeTrue();

    It should_have_reference_with_older_version_installed = () =>
      state.References.First().IsOlderVersionInstalled.ShouldBeFalse();

    It should_have_reference_with_any_version_installed = () =>
      state.References.First().IsAnyVersionInstalled.ShouldBeTrue();

    It should_have_project_name = () =>
      state.ProjectName.ShouldEqual("TestProject");
  }
}
