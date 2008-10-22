using System;
using System.Collections.Generic;

using DependencyStore.Commands;

using Machine.Specifications;

namespace DependencyStore
{
  public class with_add_new_version_command : with_testing_repository_and_blank_project
  {
    protected static AddNewVersionCommand command;

    Establish context = () =>
      command = container.Resolve.Object<AddNewVersionCommand>();
  }

  [Subject("Adding new versions")]
  public class when_adding_new_version_with_no_build_directory : with_add_new_version_command
  {
    static CommandStatus status;

    Because of = () =>
      status = command.Run();

    It should_fail = () =>
      status.ShouldEqual(CommandStatus.Failure);
  }

  [Subject("Adding new versions")]
  public class when_adding_new_version_to_blank_repository : with_add_new_version_command
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
  public class when_adding_a_second_version_to_existing_repository : with_add_new_version_command
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
}
