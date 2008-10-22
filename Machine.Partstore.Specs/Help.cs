using System;
using System.Collections.Generic;

using Machine.Partstore.Commands;

using Machine.Specifications;

namespace Machine.Partstore
{
  [Subject("Help")]
  public class when_getting_help : with_testing_repository_and_blank_project
  {
    static HelpCommand command;
    static CommandStatus status;

    Establish context = () =>
      command = container.Resolve.Object<HelpCommand>();

    Because of = () =>
      status = command.Run();

    It should_succeed = () =>
      status.ShouldEqual(CommandStatus.Success);
  }
}
