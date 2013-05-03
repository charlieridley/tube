using System;

namespace Tube
{
    public interface IPipeline<T>
    {
        T Run(string taskName, T context);
        IPipeline<T> RegisterTask(ITask<T> task);
        event EventHandler<JobUpdatedEventArgs<T>> JobUpdated;
    }
}