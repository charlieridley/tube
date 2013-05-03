using System;
using System.Collections.Generic;

namespace Tube
{
    public class Pipeline<T> : IPipeline<T>
    {
        private readonly ITaskOrderer taskOrderer;
        private readonly List<ITask<T>> tasks = new List<ITask<T>>();

        public Pipeline(ITaskOrderer taskOrderer)
        {
            this.taskOrderer = taskOrderer;
        }

        public IPipeline<T> RegisterTask(ITask<T> task)
        {
            tasks.Add(task);
            task.JobUpdated += OnJobUpdated;
            return this;
        }        

        public event EventHandler<JobUpdatedEventArgs<T>> JobUpdated;

        public T Run(string taskName, T context)
        {
            var taskPipeline = taskOrderer.Order(tasks);

            foreach (var task in taskPipeline)
            {
                task.Execute(context);
            }

            return context;
        }

        private void OnJobUpdated(object sender, JobUpdatedEventArgs<T> e)
        {
           if (JobUpdated != null)
           {
               JobUpdated(this, new JobUpdatedEventArgs<T>(e.Context));
           }
        }
    }
}
