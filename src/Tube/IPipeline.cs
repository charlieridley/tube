using System;

namespace Tube
{
    public interface IPipeline<TContext>
    {
        TContext Run(string taskName, TContext context);
        IPipeline<TContext> RegisterTask<TTask>() where TTask : ITask<TContext>;
        IPipeline<TContext> RegisterExceptionTask<TTask>() where TTask : IExceptionTask<TContext>;
        void PublishMessage<TMessage>(TMessage message);
        void Subscribe<TMessage>(Action<TMessage> subscriber);
    }
}