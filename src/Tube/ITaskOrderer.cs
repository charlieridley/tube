using System;
using System.Collections.Generic;

namespace Tube
{
    public interface ITaskOrderer
    {
        IEnumerable<Type> Order(string taskName, IEnumerable<Type> tasks);
    }
}
