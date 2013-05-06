using System;
using System.Linq;

namespace Tube
{
    public abstract class Task<TContext> : ITask<TContext>
    {
        private IPipeline<TContext> pipeline;
        public abstract void Execute(TContext context);

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