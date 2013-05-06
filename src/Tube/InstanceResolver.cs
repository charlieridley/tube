using System;

namespace Tube
{
    public class InstanceResolver : IInstanceResolver
    {
        public object Create(Type type)
        {
            return Activator.CreateInstance(type);
        }
    }
}