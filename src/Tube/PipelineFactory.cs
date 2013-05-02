namespace Tube
{
    public static class PipelineFactory
    {
        public static IPipeline<T> Create<T>()
        {
            return new Pipeline<T>(new TaskOrderer());
        }
    }
}
