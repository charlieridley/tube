namespace Tube
{
    public interface ITask<TContext>
    {
        void Execute(TContext job);
        string GetName();
        string[] GetDependencies();
        void RegisterPipeline(IPipeline<TContext> pipeline);
        void SendUpdate<TMessage>(TMessage message);
    }
}
