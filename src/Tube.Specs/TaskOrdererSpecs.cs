using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;
namespace Tube.Specs
{
    [Subject(typeof(TaskOrderer))]
    public class task_with_no_dependencies : WithSubject<TaskOrderer>
    {
        private static List<Type> tasks = new List<Type>();
        private static IEnumerable<Type> result;
        Establish context = () => tasks.Add(typeof(TaskOrdererTask1));
        Because of = () => result = Subject.Order("task1", tasks);
        It should_return_the_task = () => result.ShouldContainOnly(typeof(TaskOrdererTask1));
    }

    [Subject(typeof (TaskOrderer))]
    public class task_with_dependency : WithSubject<TaskOrderer>
    {
        private static List<Type> tasks = new List<Type>();
        private static Mock<ITask<FakeTaskContext>> task1 = new Mock<ITask<FakeTaskContext>>();
        private static Mock<ITask<FakeTaskContext>> task2 = new Mock<ITask<FakeTaskContext>>();
        private static IEnumerable<Type> result;
        Establish context = () =>
            {
                task1.Setup(x => x.GetName()).Returns("task1");
                task2.Setup(x => x.GetName()).Returns("task2");
                task2.Setup(x => x.GetDependencies()).Returns(new[]{"task1"});
                tasks.Add(typeof(TaskOrdererTask1));
                tasks.Add(typeof(TaskOrdererTask2));
                
            };
        Because of = () => result = Subject.Order("task2", tasks);
        It should_return_the_tasks_in_order = () =>
            {
                result.First().ShouldEqual(typeof(TaskOrdererTask1));
                result.Last().ShouldEqual(typeof(TaskOrdererTask2));
            };
    }

    [Subject(typeof(TaskOrderer))]
    public class task_with_two_levels_of_dependencies : WithSubject<TaskOrderer>
    {
        private static List<Type> tasks = new List<Type>();
        private static IEnumerable<Type> result;
        Establish context = () =>
        {
            tasks.Add(typeof(TaskOrdererTask2));
            tasks.Add(typeof(TaskOrdererTask1));
            tasks.Add(typeof(TaskOrdererTask3));

        };
        Because of = () => result = Subject.Order("task3", tasks);
        It should_return_the_tasks_in_order = () =>
        {
            result.ToArray()[0].ShouldEqual(typeof(TaskOrdererTask1));
            result.ToArray()[1].ShouldEqual(typeof(TaskOrdererTask2));
            result.ToArray()[2].ShouldEqual(typeof(TaskOrdererTask3));
        };
    }

    [Subject(typeof(TaskOrderer))]
    public class task_with_multiple_dependencies : WithSubject<TaskOrderer>
    {
        private static List<Type> tasks = new List<Type>();
        private static IEnumerable<Type> result;
        Establish context = () =>
        {
            tasks.Add(typeof(TaskOrdererTask2));
            tasks.Add(typeof(TaskOrdererTask1));
            tasks.Add(typeof(TaskOrdererTask4));

        };
        Because of = () => result = Subject.Order("task4", tasks);
        It should_return_the_tasks_in_order = () =>
        {
            result.ToArray()[0].ShouldEqual(typeof(TaskOrdererTask1));
            result.ToArray()[1].ShouldEqual(typeof(TaskOrdererTask2));
            result.ToArray()[2].ShouldEqual(typeof(TaskOrdererTask4));
        };
    }

    [Subject(typeof (TaskOrderer))]
    public class circular_dependencies : WithSubject<TaskOrderer>
    {
        private static List<Type> tasks = new List<Type>();     
        private static Exception exception;
        Establish context = () =>
        {
            tasks.Add(typeof(TaskOrdererTask5));
            tasks.Add(typeof(TaskOrdererTask6));

        };
        Because of = () => exception = Catch.Exception(() => Subject.Order("task5", tasks).ToList());
        It should_throw_exception = () => 
            exception.Message.ShouldEqual("There were circular dependencies");
    }

    [Subject(typeof (TaskOrderer))]
    public class indirect_circular_dependencies : WithSubject<TaskOrderer>
    {
        private static List<Type> tasks = new List<Type>();
        private static Exception exception;
        Establish context = () =>
        {

            tasks.Add(typeof(TaskOrdererTask7));
            tasks.Add(typeof(TaskOrdererTask8));
            tasks.Add(typeof(TaskOrdererTask9));
        };
        Because of = () => exception = Catch.Exception(() => Subject.Order("task7", tasks).ToList());
        It should_throw_exception = () =>
            exception.Message.ShouldEqual("There were circular dependencies");
    }
}
