namespace Tube
{
    public interface ITask<TContext> : ITaskBase<TContext>
    {
        void Execute(TContext job);
    }
}
