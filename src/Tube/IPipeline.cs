using System;

namespace Tube
{
    public interface IPipeline<T>
    {
        T Run(T context);
        IPipeline<T> RegisterTask(ITask<T> task);
        event EventHandler<JobUpdatedEventArgs<T>> JobUpdated;
    }
}