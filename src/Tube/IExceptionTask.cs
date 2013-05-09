using System;

namespace Tube
{
    public interface IExceptionTask<TContext> : ITaskBase<TContext>
    {
        void Execute(ITask<TContext> failedTask, TContext context, Exception exception);
    }
}