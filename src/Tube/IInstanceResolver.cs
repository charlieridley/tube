using System;

namespace Tube
{
    public interface IInstanceResolver
    {
        object Create(Type type);
    }
}
