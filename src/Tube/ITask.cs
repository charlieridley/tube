using System;

namespace Tube
{
    public interface ITask<T>
    {
        void Execute(T job);
        event EventHandler<JobUpdatedEventArgs<T>> JobUpdated;
        string GetName();
        string[] GetDependencies();
    }
}
