﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Tube
{
    public class TaskOrderer : ITaskOrderer
    {
        public IEnumerable<Type> Order(string taskName, IEnumerable<Type> tasks)
        {
            var adjacencyList = tasks.ToDictionary(x => x.GetTaskName(), x => x.GetDependencies().ToList());
            var taskDictionary = tasks.ToDictionary(x => x.GetTaskName(), x => x);
            return Order(adjacencyList)
                .TakeWhile(s => s != taskName)
                .Concat(new[] { taskName })                
                .Select(name => taskDictionary[name]);
        }

        private IEnumerable<string> Order(IDictionary<string, List<string>> adjacencyList)
        {
            var sortedList = new List<string>();
            var noDependencies = adjacencyList.Where(x => x.Value == null || !x.Value.Any()).ToList();

            while (noDependencies.Any())
            {
                var checking = noDependencies.First();
                sortedList.Add(checking.Key);
                noDependencies.Remove(checking);

                foreach (var node in adjacencyList.Where(x => x.Value.Contains(checking.Key)))
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
