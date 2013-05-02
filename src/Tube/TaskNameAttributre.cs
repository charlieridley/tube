using System;

namespace Tube
{
    public class TaskNameAttribute : Attribute
    {
        public string Name { get; set; }

        public TaskNameAttribute(string name)
        {
            Name = name;            
        }
    }
}
