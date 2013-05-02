using System;
using System.Collections.Generic;
using System.Linq;

namespace Tube
{
    public class TaskOrderer : ITaskOrderer
    {
        public IEnumerable<ITask<T>> Order<T>(string taskName, IDictionary<string, ITask<T>> taskHash)
        {
            return Order(taskName, taskHash, null, new List<Relationship>());
        }
        public ITask<T>[] Order<T>(string taskName, IDictionary<string, ITask<T>> taskHash, string parentTaskName, IList<Relationship> visitedRelationships)
        {
            var relationship = new Relationship(parentTaskName, taskName);
            if (visitedRelationships.Contains(relationship))
            {
                throw new InvalidOperationException("There were circular dependencies");
            }

            visitedRelationships.Add(relationship);
            var task = taskHash.Single(x => x.Key == taskName).Value;
            return task.GetDependencies()
                       .Select(x => Order(x, taskHash, taskName, visitedRelationships))
                       .SelectMany(x => x)
                       .Concat(new[] {task})
                       .Distinct()
                       .ToArray();
        }
    }
}
