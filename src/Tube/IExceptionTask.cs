using System;

namespace Tube
{
    public interface IExceptionTask<TContext>
    {
        void Execute(ITask<TContext> failedTask, TContext context, Exception exception);       
        void RegisterPipeline(IPipeline<TContext> pipeline);
        void PublishMessage<TMessage>(TMessage message);
    }
}