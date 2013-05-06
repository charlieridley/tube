using System;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;

namespace Tube.Specs
{
    [Subject(typeof(TaskTypeExtensions))]
    public class get_task_name 
    {
        It should_have_the_name_defined_in_the_attribute = () => typeof(FakeTask).GetTaskName().ShouldEqual("fake task");
    }

    [Subject(typeof(Task<FakeTaskContext>))]
    public class get_name_when_there_isnt_one 
    {
        private static Exception exception;
        Because of = () => exception = Catch.Exception(() => typeof(NamelessFakeTask).GetTaskName());
        It should_throw_an_exception = () => exception.Message.ShouldEqual("Task 'NamelessFakeTask' needs to be decorated with a TaskNameAttribute");
    }

    [Subject(typeof(Task<FakeTaskContext>))]
    public class get_dependencies : WithSubject<FakeTask>
    {
        It should_have_the_dependencies_defined_in_the_attribute = () =>
        {
            typeof(FakeTask).GetDependencies().First().ShouldEqual("task1");
            typeof(FakeTask).GetDependencies().Last().ShouldEqual("task2");
        };
    }

    [Subject(typeof(Task<FakeTaskContext>))]
    public class get_dependencies_when_there_are_none {
        It should_have_the_dependencies_defined_in_the_attribute = () => typeof(SimpleFakeTask).GetDependencies().Count().ShouldEqual(0);
    }
}
