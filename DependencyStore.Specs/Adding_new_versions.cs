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
  public class with_add_new_version_command : with_testing_repository_and_blank_project
  {
    protected static AddNewVersionCommand command;

    Establish context = () =>
      command = container.Resolve.Object<AddNewVersionCommand>();
  }

  [Subject("Adding new versions")]
  public class when_running_add_new_version_with_no_build_directory : with_add_new_version_command
  {
    static CommandStatus status;

    Because of = () =>
      status = command.Run();

    It should_fail = () =>
      status.ShouldEqual(CommandStatus.Failure);
  }

  [Subject("Adding new versions")]
  public class when_running_adding_new_version_to_blank_repository : with_add_new_version_command
  {
    static CommandStatus status;

    Establish context = () =>
      project.AddBuild();

    Because of = () =>
      status = command.Run();

    It should_succeed = () =>
      status.ShouldEqual(CommandStatus.Success);

    It should_create_directories_in_the_repository = () =>
      repository.NumberOfChildDirectories.ShouldEqual(1);
  }

  [Subject("Adding new versions")]
  public class when_running_adding_a_second_version_to_existing_repository : with_add_new_version_command
  {
    static CommandStatus status;

    Establish context = () =>
      project.AddBuild();

    Because of = () =>
    {
      status = command.Run();
      System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1.0));
      status = command.Run();
    };

    It should_succeed = () =>
      status.ShouldEqual(CommandStatus.Success);

    It should_create_directories_in_the_repository = () =>
      repository.NumberOfChildDirectories.ShouldEqual(2);
  }

  [Subject("Adding new versions")]
  public class when_adding_new_version_with_no_build_directory : with_configuration
  {
    static RepositorySets repositorySets;
    static AddingVersionResponse response;

    Establish context = () =>
    {
      CurrentProject currentProject = New.CurrentProject(New.ManifestStore(), New.RepositorySet());
      
      SetupResult.For(services.ConfigurationRepository.FindProjectConfiguration()).Return(configuration);
      SetupResult.For(services.CurrentProjectRepository.FindCurrentProject()).Return(currentProject);
      mocks.ReplayAll();

      repositorySets = container.Resolve.Object<RepositorySets>();
    };

    Because of = () =>
      response = repositorySets.AddNewVersion(String.Empty, String.Empty);

    It should_respond_no_build_directory = () => 
      response.NoBuildDirectory.ShouldBeTrue();
  }

  [Subject("Adding new versions")]
  public class when_adding_new_version_with_multiple_repositories : with_configuration
  {
    static RepositorySets repositorySets;
    static AddingVersionResponse response;

    Establish context = () =>
    {
      RepositorySet repositorySet = New.RepositorySet().With(New.Repository("Test1")).With(New.Repository("Test2"));
      CurrentProject currentProject = New.CurrentProject(New.ManifestStore(), repositorySet).WithBuild(@"C:\Temp\Build");
      
      SetupResult.For(services.ConfigurationRepository.FindProjectConfiguration()).Return(configuration);
      SetupResult.For(services.CurrentProjectRepository.FindCurrentProject()).Return(currentProject);
      mocks.ReplayAll();

      repositorySets = container.Resolve.Object<RepositorySets>();
    };

    Because of = () =>
      response = repositorySets.AddNewVersion(String.Empty, String.Empty);

    It should_respond_has_build_directory = () => 
      response.NoBuildDirectory.ShouldBeFalse();

    It should_respond_ambiguous_repositories = () => 
      response.AmbiguousRepositoryName.ShouldBeTrue();
  }

  [Subject("Adding new versions")]
  public class when_adding_new_version : with_configuration
  {
    static RepositorySets repositorySets;
    static AddingVersionResponse response;

    Establish context = () =>
    {
      RepositorySet repositorySet = New.RepositorySet().With(New.Repository("Test1"));
      CurrentProject currentProject = New.CurrentProject(New.ManifestStore(), repositorySet).WithBuild(@"C:\Temp\Build");
      
      SetupResult.For(services.ConfigurationRepository.FindProjectConfiguration()).Return(configuration);
      SetupResult.For(services.CurrentProjectRepository.FindCurrentProject()).Return(currentProject);
      SetupResult.For(services.FileSystemEntryRepository.FindEntry(Purl.For(@"C:\Temp\Build"))).Return(New.Directory());
      services.CurrentProjectRepository.SaveCurrentProject(currentProject);
      mocks.ReplayAll();

      repositorySets = container.Resolve.Object<RepositorySets>();
    };

    Because of = () =>
      response = repositorySets.AddNewVersion(String.Empty, String.Empty);

    It should_respond_has_build_directory = () => 
      response.NoBuildDirectory.ShouldBeFalse();

    It should_respond_ambiguous_repositories = () => 
      response.AmbiguousRepositoryName.ShouldBeFalse();
  }
}
