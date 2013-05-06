using System;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace Tube.Specs
{   
    [Subject(typeof(Task<FakeTaskContext>))]
    public class publish_message_without_registering_pipeline : WithSubject<FakeTask>
    {
        private static Exception exception;
        Because of = () => exception = Catch.Exception(() => Subject.PublishMessage(new FakeMessage()));
        It should_throw_an_exception = () => exception.Message.ShouldEqual("No pipeline is registered for 'FakeTask'");
    }

    [Subject(typeof(Task<FakeTask>))]
    public class publish_message : WithSubject<FakeTask>
    {
        private static FakeMessage message = new FakeMessage();
        private static Mock<IPipeline<FakeTaskContext>> pipeline = new Mock<IPipeline<FakeTaskContext>>();
        Establish context = () => Subject.RegisterPipeline(pipeline.Object);
        Because of = () => Subject.PublishMessage(message);
        It should_publish_the_message_to_the_pipeline = () => pipeline.Object.WasToldTo(x => x.PublishMessage(message));
    }
}
