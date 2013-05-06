namespace Tube
{
    public static class PipelineFactory
    {
        private static IPipelineConfiguration pipelineConfiguration = new PipelineConfiguration(new InstanceResolver());
        public static IPipelineConfiguration Configure()
        {
            return pipelineConfiguration;
        }

        public static IPipeline<TContext> Create<TContext>()
        {
            return new Pipeline<TContext>(new TaskOrderer(), pipelineConfiguration.InstanceResolver);
        }
    }
}
