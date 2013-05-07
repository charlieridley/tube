namespace Tube
{
    public abstract class Task<TContext> : TaskBase<TContext>, ITask<TContext>
    {        
        public abstract void Execute(TContext context);       
    }
}