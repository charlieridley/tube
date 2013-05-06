using System;
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
        private static FakeTask task;
        private static FakeTaskContext job = new FakeTaskContext();
        private static FakeTaskContext result;
        Establish context = () =>
            {
                task = new FakeTask();
                Subject.RegisterTask<FakeTask>();
                The<IInstanceResolver>().WhenToldTo(x => x.Create(typeof(FakeTask))).Return(task);
                The<ITaskOrderer>().WhenToldTo(x => x.Order(Param<string>.IsAnything, Param<IEnumerable<Type>>.IsAnything)).Return(new[] { typeof(FakeTask) });      
            };
        Because of = () => result = Subject.Run("fake task", job);        
        It should_create_an_instance_of_the_task = () => The<IInstanceResolver>().WasToldTo(x => x.Create(typeof(FakeTask)));
        It should_calculate_the_task_order = () => The<ITaskOrderer>().WasToldTo(x => x.Order("fake task", Param<IEnumerable<Type>>.Matches(m => m.First() == typeof(FakeTask))));
        It should_execute_the_task = () => task.Executed.ShouldEqual(job);
        It should_return_the_context = () => result.ShouldEqual(job);
    }

    [Subject(typeof(Pipeline<FakeTaskContext>))]
    public class when_a_message_is_published_with_one_subscriber : WithSubject<Pipeline<FakeTaskContext>>
    {
        private static FakeMessage publishedMessage = new FakeMessage();
        private static FakeMessage receivedMesage;
        Establish context = () => Subject.Subscribe<FakeMessage>(m => receivedMesage = m);
        Because of = () => Subject.PublishMessage(publishedMessage);
        It should_receive_the_published_message = () => receivedMesage.ShouldEqual(publishedMessage);
    }
}
