using System;
using System.Linq;

namespace Tube
{
    public static class TaskTypeExtensions
    {
        public static string GetTaskName(this Type type)
        {
            var attribute = type.GetCustomAttributes(typeof(TaskNameAttribute), true).FirstOrDefault() as TaskNameAttribute;
            if (attribute == null)
            {
                throw new Exception("Task '" + type.Name + "' needs to be decorated with a TaskNameAttribute");
            }

            return attribute.Name;
        }

        public static string[] GetDependencies(this Type type)
        {
            var attribute = type.GetCustomAttributes(typeof(TaskDependsOnAttribute), true).FirstOrDefault() as TaskDependsOnAttribute;
            return attribute == null ? new string[0] : attribute.TaskNames;
        }
    }
}
