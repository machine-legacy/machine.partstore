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
}
