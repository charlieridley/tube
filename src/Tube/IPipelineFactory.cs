namespace Tube
{
    public interface IPipelineFactory
    {
        IPipelineConfiguration Configure();
        IPipeline<TContext> Create<TContext>();
    }
}