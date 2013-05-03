using System;
using System.Linq;

namespace Tube
{
    public abstract class Task<T> : ITask<T>
    {
        public abstract void Execute(T context);

        public event EventHandler<JobUpdatedEventArgs<T>> JobUpdated;

        public virtual string GetName()
        {
            var attribute = GetType().GetCustomAttributes(typeof(TaskNameAttribute), true).FirstOrDefault() as TaskNameAttribute;
            return attribute.Name;
        }

        public virtual string[] GetDependencies()
        {
            var attribute = GetType().GetCustomAttributes(typeof(TaskDependsOnAttribute), true).FirstOrDefault() as TaskDependsOnAttribute;
            return attribute == null ? new string[0] : attribute.TaskNames;
        }        

        protected void OnJobUpdated(T context)
        {
            if (JobUpdated != null)
            {
                JobUpdated(this, new JobUpdatedEventArgs<T>(context));
            }
        }
    }
}