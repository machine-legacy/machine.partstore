using System;
using System.Collections.Generic;

using DependencyStore.Application;
using DependencyStore.Commands;
using DependencyStore.Domain.Core;
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
  public class with_no_projects : with_configuration
  {
    static ProjectState projectState;
    static CurrentProject currentProject;
    static CurrentProjectState state;

    Establish context = () =>
    {
      currentProject = New.CurrentProject(New.ManifestStore());
      SetupResult.For(services.ConfigurationRepository.FindProjectConfiguration()).Return(configuration);
      SetupResult.For(services.CurrentProjectRepository.FindCurrentProject()).Return(currentProject);
      mocks.ReplayAll();

      projectState = container.Resolve.Object<ProjectState>();
    };

    Because of = () =>
      state = projectState.GetCurrentProjectState();

    It should_return_state = () =>
      state.ShouldNotBeNull();
  }
}
