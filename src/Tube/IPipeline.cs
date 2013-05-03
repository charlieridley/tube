namespace Tube
{
    public interface IPipeline<TContext>
    {
        TContext Run(string taskName, TContext context);
        IPipeline<TContext> RegisterTask(ITask<TContext> task);
        void PublishMessage<TMessage>(TMessage message);
    }
}