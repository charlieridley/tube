using System;
using System.Collections.Generic;
using System.Linq;

namespace Tube
{
    public class TaskOrderer : ITaskOrderer
    {
        public IEnumerable<ITask<T>> Order<T>(string taskName, IDictionary<string, ITask<T>> taskHash)
        {
            var task = taskHash.Single(x => x.Key == taskName).Value;
            return task.GetDependencies()
                       .Select(x => Order(x, taskHash))
                       .SelectMany(x => x)
                       .Concat(new[] { task })
                       .Distinct()
                       .ToArray();
        }
    }
}
