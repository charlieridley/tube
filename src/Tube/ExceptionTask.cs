using System;

namespace Tube
{
    public abstract class ExceptionTask<TContext> : TaskBase<TContext>, IExceptionTask<TContext>
    {
        public abstract void Execute(ITask<TContext> failedTask,TContext context, Exception exception);       
    }
}
