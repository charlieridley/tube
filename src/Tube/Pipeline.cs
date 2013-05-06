using System;
using System.Collections.Generic;
using System.Linq;

namespace Tube
{
    public class Pipeline<TContext> : IPipeline<TContext>
    {
        private readonly ITaskOrderer taskOrderer;
        private readonly List<ITask<TContext>> tasks = new List<ITask<TContext>>();
        private readonly Dictionary<Type, List<object>> subscribers = new Dictionary<Type, List<object>>();
        public Pipeline(ITaskOrderer taskOrderer)
        {
            this.taskOrderer = taskOrderer;
        }


        public IPipeline<TContext> RegisterTask(ITask<TContext> task)
        {
            tasks.Add(task);           
            return this;
        }

        public TContext Run(string taskName, TContext context)
        {
            var taskPipeline = taskOrderer.Order(taskName, tasks);
            foreach (var task in taskPipeline)
            {
                task.Execute(context);
            }

            return context;
        }
        
        public void Subscribe<TMessage>(Action<TMessage> subscriber)
        {
            if (!subscribers.ContainsKey(typeof(TMessage)))
            {
                subscribers.Add(typeof(TMessage), new List<object>());
            }

            subscribers[typeof(TMessage)].Add(subscriber);
        }

        public void PublishMessage<TMessage>(TMessage message)
        {
            var subscribersForType = subscribers[typeof(TMessage)].Cast<Action<TMessage>>();
            foreach (var subscription in subscribersForType)
            {
                subscription(message);
            }
        }
    }
}
