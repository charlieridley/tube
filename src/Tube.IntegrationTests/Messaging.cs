using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Machine.Specifications;

namespace Tube.IntegrationTests
{
    [TaskName("message")]
    public class MessageTask : Task<CakeMaker>
    {
        public override void Execute(CakeMaker context)
        {
            PublishMessage(new FakeMessage{Message = "Hi"});
        }
    }

    public class FakeMessage
    {
        public string Message { get; set; }
    }

    public class sending_a_message
    {
        private static IPipeline<CakeMaker> pipeline;
        private static FakeMessage message;
        Establish context = () =>
            {
                pipeline = new PipelineFactory().Create<CakeMaker>()
                                                .RegisterTask<MessageTask>();
                pipeline.Subscribe<FakeMessage>(m => message = m);
            };
        Because of = () => pipeline.Run("message", new CakeMaker());
        It should_have_received_the_message = () => message.Message.ShouldEqual("Hi");
    }
}
