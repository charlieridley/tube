namespace Tube
{
    public interface ITaskBase<TContext>
    {
        void RegisterPipeline(IPipeline<TContext> pipeline);
        void PublishMessage<TMessage>(TMessage message);
    }
}