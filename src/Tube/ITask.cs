using System;

namespace Tube
{
    public interface ITask<TContext>
    {
        void Execute(TContext job);
        string GetName();
        string[] GetDependencies();
    }
}
