using System;

namespace Tube
{
    public class JobUpdatedEventArgs<T> : EventArgs
    {        
        public JobUpdatedEventArgs(T context)
        {
            Context = context;
        }

        public T Context { get; set; }
    }
}
