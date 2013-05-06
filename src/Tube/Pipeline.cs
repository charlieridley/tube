using System;
using System.Collections.Generic;
using System.Linq;

namespace Tube
{
    public class Pipeline<TContext> : IPipeline<TContext>
    {
        private readonly ITaskOrderer taskOrderer;
        private readonly IInstanceResolver instanceResolver;
        private readonly List<ITask<TContext>> taskInstances = new List<ITask<TContext>>();
        private readonly List<Type> tasks = new List<Type>();
        private readonly Dictionary<Type, List<object>> subscribers = new Dictionary<Type, List<object>>();
        public Pipeline(ITaskOrderer taskOrderer, IInstanceResolver instanceResolver)
        {
            this.taskOrderer = taskOrderer;
            this.instanceResolver = instanceResolver;
        }


        public IPipeline<TContext> RegisterTask(ITask<TContext> task)
        {
            taskInstances.Add(task);           
            return this;
        }

        public IPipeline<TContext> RegisterTask<TTask>() 
            where TTask : ITask<TContext>
        {
            tasks.Add(typeof(TTask));
            return this;
        }

        public TContext Run(string taskName, TContext context)
        {
            var taskPipeline = taskOrderer.Order(taskName, tasks);
            foreach (var taskType in taskPipeline)
            {
                var task = instanceResolver.Create(taskType) as ITask<TContext>;
                task.RegisterPipeline(this);
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
