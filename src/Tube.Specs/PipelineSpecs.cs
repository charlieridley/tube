using System.Collections.Generic;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace Tube.Specs
{
    [Subject(typeof(Pipeline<FakeTaskContext>))]
    public class running_one_task : WithSubject<Pipeline<FakeTaskContext>>
    {
        private static Mock<ITask<FakeTaskContext>> task;
        private static FakeTaskContext job = new FakeTaskContext();
        private static FakeTaskContext result;
        Establish context = () =>
            {
                task = new Mock<ITask<FakeTaskContext>>();
                task.Setup(x => x.GetName()).Returns("task1");
                Subject.RegisterTask(task.Object);
                The<ITaskOrderer>().WhenToldTo(x => x.Order(Param<string>.IsAnything, Param<IDictionary<string, ITask<FakeTaskContext>>>.IsAnything)).Return(new[] { task.Object });      
            };
        Because of = () => result = Subject.Run("task1", job);        
        It should_execute_the_task = () => task.Object.WasToldTo(x => x.Execute(job));
        It should_return_the_context = () => result.ShouldEqual(job);
    }

    [Subject(typeof (Pipeline<FakeTaskContext>))]
    public class task_has_dependencies : WithSubject<Pipeline<FakeTaskContext>>
    {
        private static Mock<ITask<FakeTaskContext>> firstTask;
        private static Mock<ITask<FakeTaskContext>> secondTask;
        private static FakeTaskContext job = new FakeTaskContext();
        private static JobUpdatedEventArgs<FakeTaskContext> eventArgs;
        Establish context = () =>
        {
            firstTask = new Mock<ITask<FakeTaskContext>>();
            firstTask.Setup(x => x.GetName()).Returns("task1");
            secondTask = new Mock<ITask<FakeTaskContext>>();
            secondTask.Setup(x => x.GetName()).Returns("task2");
            Subject.RegisterTask(firstTask.Object);
            Subject.RegisterTask(secondTask.Object);
            The<ITaskOrderer>().WhenToldTo(x => x.Order(Param<string>.IsAnything, Param<IDictionary<string, ITask<FakeTaskContext>>>.IsAnything)).Return(new[] { firstTask.Object, secondTask.Object });      
        };

        Because of = () => Subject.Run("task2", job);
        It should_calculate_the_task_order = () => The<ITaskOrderer>().WasToldTo(x => x.Order("task2", Param<IDictionary<string, ITask<FakeTaskContext>>>.Matches(m => m.First().Value == firstTask.Object && m.Last().Value == secondTask.Object)));
        It should_execute_the_first_task = () => firstTask.Object.WasToldTo(x => x.Execute(job));
        It should_execute_the_second_task = () => secondTask.Object.WasToldTo(x => x.Execute(job));
    }

    [Subject(typeof (Pipeline<FakeTaskContext>))]
    public class when_a_message_is_published_with_one_subscriber : WithSubject<Pipeline<FakeTaskContext>>
    {
        private static FakeMessage publishedMessage = new FakeMessage();
        private static FakeMessage receivedMesage;
        Establish context = () => Subject.Subscribe<FakeMessage>(m => receivedMesage = m);
        Because of = () => Subject.PublishMessage(publishedMessage);
        It should_receive_the_published_message = () => receivedMesage.ShouldEqual(publishedMessage);
    }
    
    [Subject(typeof (Pipeline<FakeTaskContext>))]
    public class when_a_message_is_published_with_two_subscribers : WithSubject<Pipeline<FakeTaskContext>>
    {
        private static FakeMessage publishedMessage = new FakeMessage();
        private static FakeMessage receivedMesage1;
        private static FakeMessage receivedMesage2;
        Establish context = () =>
            {
                Subject.Subscribe<FakeMessage>(m => receivedMesage1 = m);
                Subject.Subscribe<FakeMessage>(m => receivedMesage2 = m);
            };
        Because of = () => Subject.PublishMessage(publishedMessage);
        It should_receive_the_first_published_message = () => receivedMesage1.ShouldEqual(publishedMessage);
        It should_receive_the_second_published_message = () => receivedMesage2.ShouldEqual(publishedMessage);
    }
}
