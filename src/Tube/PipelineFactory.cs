namespace Tube
{
    public class PipelineFactory : IPipelineFactory
    {
        private IPipelineConfiguration pipelineConfiguration = new PipelineConfiguration(new InstanceResolver());
        public IPipelineConfiguration Configure()
        {
            return pipelineConfiguration;
        }

        public IPipeline<TContext> Create<TContext>()
        {
            return new Pipeline<TContext>(new TaskOrderer(), pipelineConfiguration.InstanceResolver);
        }
    }
}
