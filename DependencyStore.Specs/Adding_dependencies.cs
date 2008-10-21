using System;
using System.Collections.Generic;

using DependencyStore.Commands;

using Machine.Specifications;

namespace DependencyStore
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
}
