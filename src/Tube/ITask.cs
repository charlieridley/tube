namespace Tube
{
    public interface ITask<TContext>
    {
        void Execute(TContext job);        
        void PublishMessage<TMessage>(TMessage message);
        void RegisterPipeline(IPipeline<TContext> pipeline);
    }
}
