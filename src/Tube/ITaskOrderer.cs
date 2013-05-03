using System.Collections.Generic;

namespace Tube
{
    public interface ITaskOrderer
    {
        IEnumerable<ITask<T>> Order<T>(IEnumerable<ITask<T>> tasks);
    }
}
