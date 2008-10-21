using System;
using System.Collections.Generic;

using DependencyStore.Commands;

using Machine.Specifications;

namespace DependencyStore
{
  public class with_show_command : with_testing_repository_and_blank_project
  {
    protected static ShowCommand command;

    Establish context = () =>
      command = container.Resolve.Object<ShowCommand>();
  }

  [Subject("Configuring")]
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

  [Subject("Configuring")]
  public class when_showing_blank_project : with_configure_command
  {
    static CommandStatus status;

    Because of = () =>
      status = command.Run();

    It should_succeed = () =>
      status.ShouldEqual(CommandStatus.Success);
  }
}
