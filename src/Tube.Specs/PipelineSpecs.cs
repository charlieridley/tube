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

    [Subject(typeof(Pipeline<FakeTaskContext>))]
    public class when_a_task_throws_an_exception_an_exception_task_is_registered : WithSubject<Pipeline<FakeTaskContext>>
    {
        private static ThrowingExceptionTask task;
        private static FakeExceptionTask exceptionTask;
        private static NeverGetsRunTask noRunTask;
        private static FakeTaskContext fakeContext = new FakeTaskContext();
        Establish context = () =>
            {
                task = new ThrowingExceptionTask();
                noRunTask = new NeverGetsRunTask();
                exceptionTask = new FakeExceptionTask();
                Subject.RegisterTask<ThrowingExceptionTask>()
                       .RegisterTask<NeverGetsRunTask>()
                       .RegisterExceptionTask<FakeExceptionTask>();
                The<IInstanceResolver>().WhenToldTo(x => x.Create(typeof(FakeExceptionTask))).Return(exceptionTask);
                The<IInstanceResolver>().WhenToldTo(x => x.Create(typeof(ThrowingExceptionTask))).Return(task);
                The<IInstanceResolver>().WhenToldTo(x => x.Create(typeof(NeverGetsRunTask))).Return(noRunTask);
                The<ITaskOrderer>().WhenToldTo(x => x.Order(Param<string>.IsAnything, Param<IEnumerable<Type>>.IsAnything)).Return(new[] { typeof(ThrowingExceptionTask), typeof(NeverGetsRunTask) });      
            };
        Because of = () => Subject.Run("exception path", fakeContext);
        It should_create_an_instance_of_the_exception_task = () => The<IInstanceResolver>().WasToldTo(x => x.Create(typeof(FakeExceptionTask)));
        It should_execute_with_the_task_which_failed = () => exceptionTask.FailedTask.ShouldEqual(task);
        It should_execute_the_exception_task_with_the_context = () => exceptionTask.Context.ShouldEqual(fakeContext);
        It should_execute_the_exception_task_with_the_exception = () => exceptionTask.Exception.Message.ShouldEqual("nooooo");
        It should_not_execute_the_next_task = () => noRunTask.DidRun.ShouldBeFalse();
    }

    [Subject(typeof(Pipeline<FakeTaskContext>))]
    public class when_a_task_throws_an_exception_an_exception_task_is_not_registered : WithSubject<Pipeline<FakeTaskContext>>
    {
        private static ThrowingExceptionTask task;        
        private static FakeTaskContext fakeContext = new FakeTaskContext();
        private static Exception exception;
        Establish context = () =>
        {
            task = new ThrowingExceptionTask();            
            Subject.RegisterTask<ThrowingExceptionTask>();            
            The<IInstanceResolver>().WhenToldTo(x => x.Create(typeof(ThrowingExceptionTask))).Return(task);
            The<ITaskOrderer>().WhenToldTo(x => x.Order(Param<string>.IsAnything, Param<IEnumerable<Type>>.IsAnything)).Return(new[] { typeof(ThrowingExceptionTask) });
        };
        Because of = () => exception = Catch.Exception(() => Subject.Run("bad", fakeContext));
        It should_rethrow_the_exception = () => exception.Message.ShouldEqual("nooooo");
    }

    [Subject(typeof(Pipeline<FakeTaskContext>))]
    public class when_a_task_throws_an_exception_and_the_exception_task_cannot_be_resolved : WithSubject<Pipeline<FakeTaskContext>>
    {
        private static ThrowingExceptionTask task;
        private static FakeTaskContext fakeContext = new FakeTaskContext();
        private static Exception exception;
        Establish context = () =>
        {
            task = new ThrowingExceptionTask();
            Subject.RegisterTask<ThrowingExceptionTask>()
                   .RegisterTask<NeverGetsRunTask>()
                   .RegisterExceptionTask<FakeExceptionTask>();
            The<IInstanceResolver>().WhenToldTo(x => x.Create(typeof(ThrowingExceptionTask))).Return(task);
            The<ITaskOrderer>().WhenToldTo(x => x.Order(Param<string>.IsAnything, Param<IEnumerable<Type>>.IsAnything)).Return(new[] { typeof(ThrowingExceptionTask) });
        };
        Because of = () => exception = Catch.Exception(() => Subject.Run("bad", fakeContext));
        It should_rethrow_the_exception = () => exception.Message.ShouldEqual("nooooo");
    }
}
