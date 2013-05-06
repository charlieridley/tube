namespace Tube
{
    internal class PipelineConfiguration : IPipelineConfiguration
    {
        public IInstanceResolver InstanceResolver { get; private set; }

        public PipelineConfiguration(IInstanceResolver instanceResolver)
        {
            this.InstanceResolver = instanceResolver;
        }

        public IPipelineConfiguration SetInstanceResolver(IInstanceResolver instanceResolver)
        {
            this.InstanceResolver = instanceResolver;
            return this;
        }
    }
}