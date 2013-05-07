using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tube.Specs
{
    [TaskName("fake task")]
    [TaskDependsOn("task1", "task2")]
    public class FakeTask : Task<FakeTaskContext>
    {
        public override void Execute(FakeTaskContext context)
        {
            this.Executed = context;
        }

        public FakeTaskContext Executed { get; private set; }
    }

    public class FakeMessage
    {
    }

    [TaskName("simple fake task")]
    public class SimpleFakeTask : Task<FakeTaskContext>
    {
        public override void Execute(FakeTaskContext context)
        {
            throw new NotImplementedException();
        }
    }

    public class NamelessFakeTask : Task<FakeTaskContext>
    {
        public override void Execute(FakeTaskContext context)
        {
            throw new NotImplementedException();
        }
    }

    [TaskName("bad")]    
    public class ThrowingExceptionTask : Task<FakeTaskContext>
    {
        public override void Execute(FakeTaskContext context)
        {
            throw new Exception("nooooo");
        }

    }

    [TaskName("exception path")]
    [TaskDependsOn("bad")]
    public class NeverGetsRunTask : Task<FakeTaskContext>
    {
        public override void Execute(FakeTaskContext context)
        {
            this.DidRun = true;
        }

        public bool DidRun { get; set; }
    }

    public class FakeExceptionTask : ExceptionTask<FakeTaskContext>
    {
        public override void Execute(ITask<FakeTaskContext> failedTask,FakeTaskContext context, Exception exception)
        {
            this.Context = context;
            this.Exception = exception;
            this.FailedTask = failedTask;
        }

        public Exception Exception { get; set; }

        public FakeTaskContext Context { get; set; }

        public ITask<FakeTaskContext> FailedTask { get; set; }
    }
}
