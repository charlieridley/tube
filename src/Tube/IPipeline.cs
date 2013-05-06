using System;

namespace Tube
{
    public interface IPipeline<T>
    {
        T Run(string taskName, T context);
        IPipeline<T> RegisterTask(ITask<T> task);        
        void PublishMessage<TMessage>(TMessage message);
    }
}