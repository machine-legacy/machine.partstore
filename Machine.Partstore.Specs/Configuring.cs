using System;
using System.Collections.Generic;

using DependencyStore.Commands;

using Machine.Specifications;

namespace DependencyStore
{
  public class with_configure_command : with_testing_repository_and_blank_directory
  {
    protected static ConfigureCommand command;

    Establish context = () =>
      command = container.Resolve.Object<ConfigureCommand>();
  }

  [Subject("Configuring")]
  public class when_configuring_a_new_project_with_no_root_directory_clue : with_configure_command
  {
    static CommandStatus status;
    static Exception error;

    Establish context = () =>
      project.RemoveRootClue();

    Because of = () =>
      error = Catch.Exception(() => status = command.Run());

    It should_fail = () =>
      error.ShouldBeOfType<InvalidOperationException>();
  }

  [Subject("Configuring")]
  public class when_configuring_a_new_project : with_configure_command
  {
    static CommandStatus status;

    Because of = () =>
      status = command.Run();

    It should_succeed = () =>
      status.ShouldEqual(CommandStatus.Success);

    It should_write_configuration_file = () =>
      project.HasConfiguration.ShouldBeTrue();
  }

  [Subject("Configuring")]
  public class when_configuring_an_existing_project : with_configure_command
  {
    static CommandStatus status;

    Because of = () =>
    {
      command.Run();
      status = command.Run();
    };

    It should_succeed = () =>
      status.ShouldEqual(CommandStatus.Success);

    It should_write_configuration_file = () =>
      project.HasConfiguration.ShouldBeTrue();
  }
}
