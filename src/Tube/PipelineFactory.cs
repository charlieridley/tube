namespace Tube
{
    public static class PipelineFactory
    {
        public static IPipeline<TContext> Create<TContext>()
        {
            return new Pipeline<TContext>(new TaskOrderer());
        }
    }
}
