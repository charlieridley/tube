using System;
using System.Collections.Generic;
using System.Linq;

namespace Tube
{
    public class TaskOrderer : ITaskOrderer
    {
        public IEnumerable<ITask<T>> Order<T>(string taskName, IEnumerable<ITask<T>> tasks)
        {
            tasks = tasks.ToArray();

            var adjacencyList = tasks.ToDictionary(kvp => kvp.GetName(), kvp => kvp.GetDependencies().ToList());
            var taskDictionary = tasks.ToDictionary(kvp => kvp.GetName(), kvp => kvp);

            return Order(adjacencyList)
                .TakeWhile(s => s != taskName)
                .Concat(new []{taskName})
                .Select(name => taskDictionary[name]);
        }

        IEnumerable<string> Order(IDictionary<string, List<string>> adjacencyList)
        {
            var sortedList = new List<string>();
            var noDependencies = adjacencyList.Where(kvp => kvp.Value == null || !kvp.Value.Any()).ToList();

            while (noDependencies.Any())
            {
                var checking = noDependencies.First();
                sortedList.Add(checking.Key);
                noDependencies.Remove(checking);

                foreach (var node in adjacencyList.Where(kvp => kvp.Value.Contains(checking.Key)))
                {
                    node.Value.Remove(checking.Key);
                    if (!node.Value.Any())
                    {
                        noDependencies.Add(node);
                    }
                }
            }

            if (adjacencyList.Any(kvp => kvp.Value.Any()))
            {
                throw new InvalidOperationException("There were circular dependencies");
            }
            
            return sortedList;
        }
    }
}
