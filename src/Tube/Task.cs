using System;
using System.Linq;

namespace Tube
{
    public abstract class Task<TContext> : ITask<TContext>
    {
        private IPipeline<TContext> pipeline;
        public abstract void Execute(TContext context);

        public virtual string GetName()
        {
            var attribute = GetType().GetCustomAttributes(typeof(TaskNameAttribute), true).FirstOrDefault() as TaskNameAttribute;
            if (attribute == null)
            {
                throw new Exception("Task '"+ this.GetType().Name + "' needs to be decorated with a TaskNameAttribute");
            }

            return attribute.Name;
        }

        public virtual string[] GetDependencies()
        {
            var attribute = GetType().GetCustomAttributes(typeof(TaskDependsOnAttribute), true).FirstOrDefault() as TaskDependsOnAttribute;
            return attribute == null ? new string[0] : attribute.TaskNames;
        }

        public void RegisterPipeline(IPipeline<TContext> pipeline)
        {
            this.pipeline = pipeline;
        }

        public void SendUpdate<TMessage>(TMessage message)
        {
            if (pipeline == null)
            {
                throw new Exception("No pipeline is registered for '" + this.GetType().Name + "'");
            }

            pipeline.PublishMessage(message);
        }
    }
}