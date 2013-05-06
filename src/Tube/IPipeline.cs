namespace Tube
{
    public interface IPipeline<TContext>
    {
        TContext Run(string taskName, TContext context);
        IPipeline<TContext> RegisterTask<TTask>() where TTask : ITask<TContext>;        
        void PublishMessage<TMessage>(TMessage message);
    }
}