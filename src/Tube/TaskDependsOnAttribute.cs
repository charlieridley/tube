using System;

namespace Tube
{
    public class TaskDependsOnAttribute : Attribute
    {        
        public TaskDependsOnAttribute(params string[] taskNames)
        {
            TaskNames = taskNames;            
        }

        public string[] TaskNames { get; set; }
    }
}
