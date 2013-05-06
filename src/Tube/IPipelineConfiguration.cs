namespace Tube
{
    public interface IPipelineConfiguration
    {
        IPipelineConfiguration SetInstanceResolver(IInstanceResolver instanceResolver);
        IInstanceResolver InstanceResolver { get; }
    }
}