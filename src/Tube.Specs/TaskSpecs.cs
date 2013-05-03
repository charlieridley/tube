using System;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace Tube.Specs
{    
    [TaskName("fake task")]
    [TaskDependsOn("task1", "task2")]
    public class FakeTask : Task<FakeTaskContext>
    {
        public override void Execute(FakeTaskContext context)
        {
            throw new System.NotImplementedException();
        }        
    }

    public class FakeMessage
    {
    }

    [TaskName("simple fake task")]
    public class SimpleFakeTask :Task<FakeTaskContext>
    {
        public override void Execute(FakeTaskContext context)
        {
            throw new NotImplementedException();
        }
    }

    public class NamelessFakeTask :Task<FakeTaskContext>
    {
        public override void Execute(FakeTaskContext context)
        {
            throw new NotImplementedException();
        }
    }

    [Subject(typeof (Task<FakeTaskContext>))]
    public class get_name : WithSubject<FakeTask>
    {
        It should_have_the_name_defined_in_the_attribute = () => Subject.GetName().ShouldEqual("fake task");
    }

    [Subject(typeof(Task<FakeTaskContext>))]
    public class get_name_when_there_isnt_one : WithSubject<NamelessFakeTask>
    {
        private static Exception exception;
        Because of = () => exception = Catch.Exception(() =>Subject.GetName().ShouldEqual("fake task"));
        It should_throw_an_exception = () => exception.Message.ShouldEqual("Task 'NamelessFakeTask' needs to be decorated with a TaskNameAttribute");
    }

    [Subject(typeof(Task<FakeTaskContext>))]
    public class get_dependencies : WithSubject<FakeTask>
    {
        It should_have_the_dependencies_defined_in_the_attribute = () =>
            {
                Subject.GetDependencies().First().ShouldEqual("task1");
                Subject.GetDependencies().Last().ShouldEqual("task2");
            };
    }

    [Subject(typeof(Task<FakeTaskContext>))]
    public class get_dependencies_when_there_are_none : WithSubject<SimpleFakeTask>
    {
        It should_have_the_dependencies_defined_in_the_attribute = () => Subject.GetDependencies().Count().ShouldEqual(0);
    }

    [Subject(typeof (Task<FakeTaskContext>))]
    public class send_update_without_registering_pipeline : WithSubject<FakeTask>
    {
        private static Exception exception;
        Because of = () => exception = Catch.Exception(() => Subject.SendUpdate(new FakeMessage()));
        It should_throw_an_exception = () => exception.Message.ShouldEqual("No pipeline is registered for 'FakeTask'");
    }

    [Subject(typeof(Task<FakeTask>))]
    public class send_update : WithSubject<FakeTask>
    {
        private static FakeMessage message = new FakeMessage(); 
        private static Mock<IPipeline<FakeTaskContext>> pipeline = new Mock<IPipeline<FakeTaskContext>>();
        Establish context = () => Subject.RegisterPipeline(pipeline.Object);
        Because of = () => Subject.SendUpdate(message);
        It should_publish_the_message_to_the_pipeline = () => pipeline.Object.WasToldTo(x => x.PublishMessage(message));
    }
}
