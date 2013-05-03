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
        private static List<ITask<FakeTaskContext>> tasks = new List<ITask<FakeTaskContext>>();
        private static Mock<ITask<FakeTaskContext>> task = new Mock<ITask<FakeTaskContext>>();
        private static IEnumerable<ITask<FakeTaskContext>> result;
        Establish context = () =>
        {
            task.Setup(x => x.GetName()).Returns("task1");
            tasks.Add(task.Object);
        };
        Because of = () => result = Subject.Order("task1", tasks);
        It should_return_the_task = () => result.ShouldContainOnly(task.Object);
    }

    [Subject(typeof (TaskOrderer))]
    public class task_with_dependency : WithSubject<TaskOrderer>
    {
        private static List<ITask<FakeTaskContext>> tasks = new List<ITask<FakeTaskContext>>();
        private static Mock<ITask<FakeTaskContext>> task1 = new Mock<ITask<FakeTaskContext>>();
        private static Mock<ITask<FakeTaskContext>> task2 = new Mock<ITask<FakeTaskContext>>();
        private static IEnumerable<ITask<FakeTaskContext>> result;
        Establish context = () =>
            {
                task1.Setup(x => x.GetName()).Returns("task1");
                task2.Setup(x => x.GetName()).Returns("task2");
                task2.Setup(x => x.GetDependencies()).Returns(new[]{"task1"});
                tasks.Add(task2.Object);
                tasks.Add(task1.Object);
                
            };
        Because of = () => result = Subject.Order("task2", tasks);
        It should_return_the_tasks_in_order = () =>
            {
                result.First().ShouldEqual(task1.Object);
                result.Last().ShouldEqual(task2.Object);
            };
    }

    [Subject(typeof(TaskOrderer))]
    public class task_with_two_levels_of_dependencies : WithSubject<TaskOrderer>
    {
        private static List<ITask<FakeTaskContext>> tasks = new List<ITask<FakeTaskContext>>();
        private static Mock<ITask<FakeTaskContext>> task1 = new Mock<ITask<FakeTaskContext>>();
        private static Mock<ITask<FakeTaskContext>> task2 = new Mock<ITask<FakeTaskContext>>();
        private static Mock<ITask<FakeTaskContext>> task3 = new Mock<ITask<FakeTaskContext>>();
        private static IEnumerable<ITask<FakeTaskContext>> result;
        Establish context = () =>
        {
            task1.Setup(x => x.GetName()).Returns("task1");
            task2.Setup(x => x.GetName()).Returns("task2"); 
            task3.Setup(x => x.GetName()).Returns("task3");
            task2.Setup(x => x.GetDependencies()).Returns(new[] { "task1" });
            task3.Setup(x => x.GetDependencies()).Returns(new[] { "task2" });
            tasks.Add(task2.Object);
            tasks.Add(task1.Object);
            tasks.Add(task3.Object);

        };
        Because of = () => result = Subject.Order("task3", tasks);
        It should_return_the_tasks_in_order = () =>
        {
            result.ToArray()[0].ShouldEqual(task1.Object);
            result.ToArray()[1].ShouldEqual(task2.Object);
            result.ToArray()[2].ShouldEqual(task3.Object);
        };
    }

    [Subject(typeof(TaskOrderer))]
    public class task_with_multiple_dependencies : WithSubject<TaskOrderer>
    {
        private static List<ITask<FakeTaskContext>> tasks = new List<ITask<FakeTaskContext>>();
        private static Mock<ITask<FakeTaskContext>> task1 = new Mock<ITask<FakeTaskContext>>();
        private static Mock<ITask<FakeTaskContext>> task2 = new Mock<ITask<FakeTaskContext>>();
        private static Mock<ITask<FakeTaskContext>> task3 = new Mock<ITask<FakeTaskContext>>();
        private static IEnumerable<ITask<FakeTaskContext>> result;
        Establish context = () =>
        {
            task1.Setup(x => x.GetName()).Returns("task1");
            task2.Setup(x => x.GetName()).Returns("task2");
            task3.Setup(x => x.GetName()).Returns("task3");
            task3.Setup(x => x.GetDependencies()).Returns(new[] { "task1", "task2" });
            tasks.Add(task2.Object);
            tasks.Add(task1.Object);
            tasks.Add(task3.Object);

        };
        Because of = () => result = Subject.Order("task3", tasks);
        It should_return_the_tasks_in_order = () =>
        {
            result.ToArray()[1].ShouldEqual(task1.Object);
            result.ToArray()[0].ShouldEqual(task2.Object);
            result.ToArray()[2].ShouldEqual(task3.Object);
        };
    }

    [Subject(typeof(TaskOrderer))]
    public class task_with_common_dependencies : WithSubject<TaskOrderer>
    {
        private static List<ITask<FakeTaskContext>> tasks = new List<ITask<FakeTaskContext>>();
        private static Mock<ITask<FakeTaskContext>> task1 = new Mock<ITask<FakeTaskContext>>();
        private static Mock<ITask<FakeTaskContext>> task2 = new Mock<ITask<FakeTaskContext>>();
        private static Mock<ITask<FakeTaskContext>> task3 = new Mock<ITask<FakeTaskContext>>();
        private static IEnumerable<ITask<FakeTaskContext>> result;
        Establish context = () =>
        {
            task1.Setup(x => x.GetName()).Returns("task1");
            task2.Setup(x => x.GetName()).Returns("task2");
            task3.Setup(x => x.GetName()).Returns("task3");

            task2.Setup(x => x.GetDependencies()).Returns(new[] { "task1" });
            task3.Setup(x => x.GetDependencies()).Returns(new[] { "task1", "task2" });
            tasks.Add(task2.Object);
            tasks.Add(task1.Object);
            tasks.Add(task3.Object);

        };
        Because of = () => result = Subject.Order("task3", tasks);
        It should_return_the_tasks_in_order = () =>
        {
            result.ToArray()[0].ShouldEqual(task1.Object);
            result.ToArray()[1].ShouldEqual(task2.Object);
            result.ToArray()[2].ShouldEqual(task3.Object);
        };

        It should_have_3_tasks = () => result.Count().ShouldEqual(3);
    }

    [Subject(typeof (TaskOrderer))]
    public class circular_dependencies : WithSubject<TaskOrderer>
    {
        private static List<ITask<FakeTaskContext>> tasks = new List<ITask<FakeTaskContext>>();
        private static Mock<ITask<FakeTaskContext>> task1 = new Mock<ITask<FakeTaskContext>>();
        private static Mock<ITask<FakeTaskContext>> task2 = new Mock<ITask<FakeTaskContext>>();        
        private static Exception exception;
        Establish context = () =>
        {
            task1.Setup(x => x.GetName()).Returns("task1");
            task2.Setup(x => x.GetName()).Returns("task2");
            task1.Setup(x => x.GetDependencies()).Returns(new[] { "task2" });
            task2.Setup(x => x.GetDependencies()).Returns(new[] { "task1" });
            tasks.Add(task2.Object);
            tasks.Add(task1.Object);

        };
        Because of = () => exception = Catch.Exception(() => Subject.Order("task3", tasks).ToList());
        It should_throw_exception = () => 
            exception.Message.ShouldEqual("There were circular dependencies");
    }

    [Subject(typeof (TaskOrderer))]
    public class indirect_circular_dependencies : WithSubject<TaskOrderer>
    {
        private static List<ITask<FakeTaskContext>> tasks = new List<ITask<FakeTaskContext>>();
        private static Mock<ITask<FakeTaskContext>> task1 = new Mock<ITask<FakeTaskContext>>();
        private static Mock<ITask<FakeTaskContext>> task2 = new Mock<ITask<FakeTaskContext>>();
        private static Mock<ITask<FakeTaskContext>> task3 = new Mock<ITask<FakeTaskContext>>();        
        private static Exception exception;
        Establish context = () =>
        {
            task1.Setup(x => x.GetName()).Returns("task1");
            task2.Setup(x => x.GetName()).Returns("task2");
            task3.Setup(x => x.GetName()).Returns("task3");
            task1.Setup(x => x.GetDependencies()).Returns(new[] { "task3" });
            task2.Setup(x => x.GetDependencies()).Returns(new[] { "task1" });
            task3.Setup(x => x.GetDependencies()).Returns(new[] { "task2" });
            tasks.Add(task2.Object);
            tasks.Add(task1.Object);
            tasks.Add(task3.Object);
        };
        Because of = () => exception = Catch.Exception(() => Subject.Order("task3", tasks).ToList());
        It should_throw_exception = () =>
            exception.Message.ShouldEqual("There were circular dependencies");
    }
}
