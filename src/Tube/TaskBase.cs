using System;

namespace Tube
{
    public abstract class TaskBase<TContext> : ITaskBase<TContext>
    {
        private IPipeline<TContext> pipeline;
        public void RegisterPipeline(IPipeline<TContext> pipeline)
        {
            this.pipeline = pipeline;
        }

        public void PublishMessage<TMessage>(TMessage message)
        {
            if (pipeline == null)
            {
                throw new Exception("No pipeline is registered for '" + this.GetType().Name + "'");
            }

            pipeline.PublishMessage(message);
        }
    }
}
