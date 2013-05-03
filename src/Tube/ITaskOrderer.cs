using System.Collections.Generic;

namespace Tube
{
    public interface ITaskOrderer
    {
        IEnumerable<ITask<T>> Order<T>(string taskName, IEnumerable<ITask<T>> tasks);
    }
}
